using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features;

[ApiController]
[Route("v2")]
public class MockKonnectController : ControllerBase
{
    private static readonly ConcurrentDictionary<string, MockPayment> _payments = new();
    private readonly ILogger<MockKonnectController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public MockKonnectController(ILogger<MockKonnectController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        
    }

    [HttpPost("payments/init-payment")]
    public async Task<IActionResult> InitPayment([FromBody] InitPaymentRequest request)
    {
        _logger.LogInformation("Mock Konnect: Creating payment for amount {Amount}", request.Amount);

        var paymentRef = $"PAY_{Guid.NewGuid():N}";
        var payUrl = $"http://localhost:5000/v2/pay/{paymentRef}";

        var payment = new MockPayment
        {
            Id = paymentRef,
            Reference = paymentRef,
            Status = "pending",
            Amount = request.Amount,
            AmountDue = request.Amount,
            ReachedAmount = 0,
            Token = request.Token,
            ExpirationDate = DateTime.UtcNow.AddMinutes(request.Lifespan).ToString("yyyy-MM-ddTHH:mm:ssZ"),
            Link = payUrl,
            Webhook = request.Webhook,
            SuccessUrl = request.SuccessUrl,
            FailUrl = request.FailUrl,
            OrderId = request.OrderId.ToString(),
            Type = request.Type,
            ReceiverWalletId = request.ReceiverWalletId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow
        };

        _payments.TryAdd(paymentRef, payment);

        var response = new
        {
            PaymentRef = paymentRef,
            PayUrl = payUrl
        };

        return Ok(response);
    }

    [HttpGet("payments/{paymentRef}")]
    public IActionResult GetPaymentDetails(string paymentRef)
    {
        _logger.LogInformation("Mock Konnect: Getting payment details for {PaymentRef}", paymentRef);

        if (!_payments.TryGetValue(paymentRef, out var payment))
        {
            return NotFound(new { error = "Payment not found" });
        }

        // Check if payment is expired
        if (DateTime.TryParse(payment.ExpirationDate, out var expirationDate) && expirationDate < DateTime.UtcNow)
        {
            payment.Status = "expired";
        }

        var response = new
        {
            id = payment.Id,
            status = payment.Status,
            amountDue = payment.AmountDue,
            reachedAmount = payment.ReachedAmount,
            amount = payment.Amount,
            token = payment.Token,
            convertedAmount = payment.Amount, // Simplified conversion
            exchangeRate = 1,
            expirationDate = payment.ExpirationDate,
            shortId = payment.Reference[^8..], // Last 8 chars
            link = payment.Link,
            webhook = payment.Webhook,
            successUrl = payment.SuccessUrl,
            failUrl = payment.FailUrl,
            orderId = payment.OrderId,
            type = payment.Type,
            details = "Mock payment for testing",
            acceptedPaymentMethods = new[] { "wallet", "bank_card" },
            receiverWallet = new { id = payment.ReceiverWalletId },
            transactions = payment.Transactions.Select(t => new { status = t.Status }).ToArray()
        };

        return Ok(response);
    }

    [HttpGet("pay/{paymentRef}")]
    public IActionResult PaymentPage(string paymentRef)
    {
        if (!_payments.TryGetValue(paymentRef, out var payment))
        {
            return NotFound("Payment not found");
        }

        if (payment.Status != "pending")
        {
            return BadRequest($"Payment is {payment.Status}");
        }

        var html = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Mock Konnect Payment</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 50px; background: #f5f5f5; }}
        .payment-container {{ background: white; padding: 30px; border-radius: 8px; max-width: 500px; margin: 0 auto; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
        .amount {{ font-size: 24px; font-weight: bold; color: #2c5aa0; margin: 20px 0; }}
        .details {{ margin: 15px 0; }}
        .btn {{ background: #2c5aa0; color: white; padding: 12px 24px; border: none; border-radius: 4px; cursor: pointer; font-size: 16px; margin: 10px 5px; }}
        .btn:hover {{ background: #1e3d6f; }}
        .wallet-option {{ margin: 10px 0; padding: 15px; border: 1px solid #ddd; border-radius: 4px; cursor: pointer; }}
        .wallet-option:hover {{ background: #f0f8ff; }}
        .wallet-balance {{ color: #666; font-size: 14px; }}
    </style>
</head>
<body>
    <div class='payment-container'>
        <h2>Mock Konnect Payment</h2>
        <div class='details'>
            <strong>Payment Reference:</strong> {payment.Reference}<br>
            <strong>Recipient:</strong> {payment.FirstName} {payment.LastName}<br>
            <strong>Email:</strong> {payment.Email}
        </div>
        <div class='amount'>Amount: ${payment.Amount / 100.0:F2}</div>
                
        <button class='btn' onclick='payWithCard()'>Pay with Card (Always Success)</button>
        <button class='btn' onclick='simulateFailure()' style='background: #dc3545;'>Simulate Failure</button>
    </div>

    <script>
        function payWithWallet(walletId) {{
            fetch(`http://localhost:5000/v2/process-payment/{paymentRef}`, {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify({{ walletId: walletId, paymentMethod: 'wallet' }})
            }})
            .then(response => response.json())
            .then(data => {{
                if (data.success) {{
                    alert('Payment successful!');
                    window.location.href = data.redirectUrl || '/';
                }} else {{
                    alert('Payment failed: ' + data.error);
                }}
            }})
            .catch(error => {{
                alert('Payment error: ' + error);
            }});
        }}

        function payWithCard() {{
            fetch(`http://localhost:5000/v2/process-payment/{paymentRef}`, {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify({{ paymentMethod: 'card' }})
            }})
            .then(response => response.json())
            .then(data => {{
                if (data.success) {{
                    alert('Payment successful!');
                    window.location.href = data.redirectUrl || '/';
                }} else {{
                    alert('Payment failed: ' + data.error);
                }}
            }});
        }}

        function simulateFailure() {{
            fetch(`/api/mock-konnect/process-payment/{paymentRef}`, {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify({{ paymentMethod: 'fail' }})
            }})
            .then(response => response.json())
            .then(data => {{
                alert('Payment failed: ' + data.error);
            }});
        }}
    </script>
</body>
</html>";

        return Content(html, "text/html");
    }

    [HttpPost("process-payment/{paymentRef}")]
    public async Task<IActionResult> ProcessPayment(string paymentRef, [FromBody] ProcessPaymentRequest request)
    {
        _logger.LogInformation("Mock Konnect: Processing payment {PaymentRef} with method {Method}", paymentRef, request.PaymentMethod);

        if (!_payments.TryGetValue(paymentRef, out var payment))
        {
            return NotFound(new { success = false, error = "Payment not found" });
        }

        /*if (payment.Status != "pending")
        {
            return BadRequest(new { success = false, error = $"Payment is {payment.Status}" });
        }*/

        // Check expiration
        if (DateTime.TryParse(payment.ExpirationDate, out var expirationDate) && expirationDate < DateTime.UtcNow)
        {
            payment.Status = "expired";
            return BadRequest(new { success = false, error = "Payment has expired" });
        }

        // Simulate different payment scenarios
        switch (request.PaymentMethod)
        {
            case "card":
                await CompletePayment(payment);
                return Ok(new { success = true, redirectUrl = payment.SuccessUrl });

            case "fail":
                payment.Status = "failed";
                await TriggerWebhook(payment);
                return BadRequest(new { success = false, error = "Payment failed (simulated)" });

            default:
                return BadRequest(new { success = false, error = "Invalid payment method" });
        }
    }

    
    private async Task CompletePayment(MockPayment payment)
    {
        payment.Status = "completed";
        payment.ReachedAmount = payment.AmountDue;
        payment.Transactions.Add(new MockTransaction { Status = "completed", Amount = payment.Amount });

        await TriggerWebhook(payment);
    }

    private async Task TriggerWebhook(MockPayment payment)
    {
        if (string.IsNullOrEmpty(payment.Webhook))
        {
            _logger.LogWarning("No webhook URL configured for payment {PaymentRef}", payment.Reference);
            return;
        }

        try
        {
            var httpClient = _httpClientFactory.CreateClient("KonnectClient");

            _logger.LogInformation("Triggering webhook for payment {PaymentRef} to {WebhookUrl}", payment.Reference, payment.Webhook);

            var response = await httpClient.GetAsync($"{payment.Webhook}?payment_ref={payment.Reference}");
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Webhook triggered successfully for payment {PaymentRef}", payment.Reference);
            }
            else
            {
                _logger.LogError("Webhook failed for payment {PaymentRef}. Status: {StatusCode}", payment.Reference, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error triggering webhook for payment {PaymentRef}", payment.Reference);
        }
    }
}

public class InitPaymentRequest
{
    [JsonPropertyName("receiverWalletId")]
    public string ReceiverWalletId { get; set; } = string.Empty;

    [JsonPropertyName("token")]
    public string Token { get; set; } = "USD";

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "immediate";

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("lifespan")]
    public int Lifespan { get; set; } = 10;

    [JsonPropertyName("checkoutForm")]
    public bool CheckoutForm { get; set; } = false;

    [JsonPropertyName("addPaymentFeesToAmount")]
    public bool AddPaymentFeesToAmount { get; set; } = true;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("orderId")]
    public int OrderId { get; set; }

    [JsonPropertyName("webhook")]
    public string Webhook { get; set; } = string.Empty;

    [JsonPropertyName("successUrl")]
    public string SuccessUrl { get; set; } = string.Empty;

    [JsonPropertyName("failUrl")]
    public string FailUrl { get; set; } = string.Empty;

    [JsonPropertyName("silentWebhook")]
    public bool SilentWebhook { get; set; } = true;

    [JsonPropertyName("theme")]
    public string Theme { get; set; } = "light";
}

public class ProcessPaymentRequest
{
    public string PaymentMethod { get; set; } = string.Empty;
}


public class MockPayment
{
    public string Id { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Status { get; set; } = "pending";
    public int Amount { get; set; }
    public int AmountDue { get; set; }
    public int ReachedAmount { get; set; }
    public string Token { get; set; } = "USD";
    public string ExpirationDate { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Webhook { get; set; } = string.Empty;
    public string SuccessUrl { get; set; } = string.Empty;
    public string FailUrl { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string ReceiverWalletId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<MockTransaction> Transactions { get; set; } = new();
}

public class MockTransaction
{
    public string Status { get; set; } = string.Empty;
    public int Amount { get; set; }
}

