import React, { useState } from 'react';
import {
  Button,
  Input,
  Alert,
} from '@/components';
import { DollarSign, AlertTriangle } from 'lucide-react';
import { cn } from '@/utils/cn';

interface PayoutRequestFormProps {
  availableBalance: number;
  onSubmit: (amount: number) => void;
  isLoading?: boolean;
  onCancel?: () => void;
}

export function PayoutRequestForm({
  availableBalance,
  onSubmit,
  isLoading = false,
  onCancel,
}: PayoutRequestFormProps) {
  const [amount, setAmount] = useState<string>('');
  const [error, setError] = useState<string>('');

  const handleAmountChange = (value: string) => {
    // Allow empty string for clearing the field
    if (value === '') {
      setAmount('');
      setError('');
      return;
    }

    // Remove any non-numeric characters except decimal point
    const cleanValue = value.replace(/[^\d.]/g, '');
    
    // Ensure only one decimal point
    const parts = cleanValue.split('.');
    if (parts.length > 2) return;
    
    // Limit to 2 decimal places
    if (parts[1] && parts[1].length > 2) {
      parts[1] = parts[1].substring(0, 2);
    }
    
    const formattedValue = parts.join('.');
    setAmount(formattedValue);

    // Validate amount
    const numericValue = parseFloat(formattedValue);
    if (isNaN(numericValue)) {
      setError('Please enter a valid amount');
      return;
    }

    if (numericValue < 20) {
      setError('Minimum payout amount is $20.00');
      return;
    }

    if (numericValue > availableBalance) {
      setError(`Amount exceeds available balance of $${availableBalance.toFixed(2)}`);
      return;
    }

    setError('');
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    const numericAmount = parseFloat(amount);
    
    if (isNaN(numericAmount) || numericAmount < 20 || numericAmount > availableBalance) {
      return;
    }

    onSubmit(numericAmount);
  };

  const isValidAmount = () => {
    const numericAmount = parseFloat(amount);
    return !isNaN(numericAmount) && 
           numericAmount >= 20 && 
           numericAmount <= availableBalance && 
           !error;
  };

  const suggestedAmounts = [20, 50, 100, Math.min(availableBalance, 500)].filter(
    (suggested) => suggested <= availableBalance && suggested >= 20
  );

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="space-y-2">
        <h3 className="text-lg font-semibold">Request Payout</h3>
        <p className="text-sm text-muted-foreground">
          Enter the amount you'd like to withdraw from your account.
        </p>
      </div>

      {/* Available Balance Display */}
      <div className="bg-muted/50 rounded-lg p-4">
        <div className="flex items-center gap-2 mb-1">
          <DollarSign className="h-4 w-4 text-muted-foreground" />
          <span className="text-sm font-medium text-muted-foreground">
            Available Balance
          </span>
        </div>
        <div className="text-2xl font-bold text-primary">
          ${availableBalance.toFixed(2)}
        </div>
      </div>

      {/* Form */}
      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="space-y-2">
          <label htmlFor="amount" className="text-sm font-medium">
            Payout Amount
          </label>
          <div className="relative">
            <div className="absolute left-3 top-1/2 -translate-y-1/2 text-muted-foreground">
              $
            </div>
            <Input
              id="amount"
              type="text"
              placeholder="0.00"
              value={amount}
              onChange={(e) => handleAmountChange(e.target.value)}
              className={cn(
                'pl-8',
                error && 'border-destructive focus-visible:border-destructive'
              )}
              disabled={isLoading}
            />
          </div>
          {error && (
            <p className="text-sm text-destructive flex items-center gap-1">
              <AlertTriangle className="h-3 w-3" />
              {error}
            </p>
          )}
        </div>

        {/* Quick Amount Buttons */}
        {suggestedAmounts.length > 0 && (
          <div className="space-y-2">
            <p className="text-sm font-medium text-muted-foreground">
              Quick amounts
            </p>
            <div className="flex flex-wrap gap-2">
              {suggestedAmounts.map((suggested) => (
                <Button
                  key={suggested}
                  type="button"
                  variant="outline"
                  size="sm"
                  onClick={() => handleAmountChange(suggested.toString())}
                  disabled={isLoading}
                  className="text-xs"
                >
                  ${suggested}
                </Button>
              ))}
              {availableBalance >= 20 && (
                <Button
                  type="button"
                  variant="outline"
                  size="sm"
                  onClick={() => handleAmountChange(availableBalance.toString())}
                  disabled={isLoading}
                  className="text-xs"
                >
                  Max (${availableBalance.toFixed(2)})
                </Button>
              )}
            </div>
          </div>
        )}

        {/* Minimum Amount Notice */}
        <Alert className="bg-blue-50 border-blue-200 dark:bg-blue-950/20 dark:border-blue-900">
          <AlertTriangle className="h-4 w-4 text-blue-600 dark:text-blue-400" />
          <div className="ml-2">
            <p className="text-sm text-blue-800 dark:text-blue-200">
              <strong>Minimum payout:</strong> $20.00
            </p>
            <p className="text-xs text-blue-700 dark:text-blue-300 mt-1">
              Payouts are typically processed within 3-5 business days.
            </p>
          </div>
        </Alert>

        {/* Form Actions */}
        <div className="flex gap-3 pt-4">
          <Button
            type="button"
            variant="outline"
            onClick={onCancel}
            disabled={isLoading}
            className="flex-1"
          >
            Cancel
          </Button>
          <Button
            type="submit"
            disabled={!isValidAmount() || isLoading}
            className="flex-1"
          >
            {isLoading ? 'Processing...' : 'Request Payout'}
          </Button>
        </div>
      </form>
    </div>
  );
}