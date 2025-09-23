import { AlertCircle, RefreshCw, MessageCircle } from 'lucide-react';
import { Button, Alert, AlertDescription } from '@/components/ui';

interface BookingErrorPageProps {
  onRetry: () => void;
  onContactSupport: () => void;
  errorMessage?: string;
}

export function BookingErrorPage({
  onRetry,
  onContactSupport,
  errorMessage,
}: BookingErrorPageProps) {
  return (
    <div className="container mx-auto py-10 px-4 max-w-4xl">
      <div className="text-center space-y-8">
        <div className="flex flex-col items-center space-y-4">
          <div className="w-24 h-24 bg-red-100 rounded-full flex items-center justify-center">
            <AlertCircle className="w-12 h-12 text-red-600" />
          </div>
          <div>
            <h1 className="text-4xl font-bold text-gray-900 mb-2">
              Booking Failed
            </h1>
            <p className="text-lg text-gray-600 max-w-2xl">
              We encountered an issue while processing your booking. This could
              be due to a network error or the time slot may have been taken by
              another user.
            </p>
          </div>
        </div>

        {/* Error Details */}
        {errorMessage && (
          <Alert variant="destructive" className="max-w-2xl mx-auto">
            <AlertCircle className="w-4 h-4" />
            <AlertDescription>
              <strong>Error Details:</strong> {errorMessage}
            </AlertDescription>
          </Alert>
        )}

        {/* Troubleshooting */}
        <div className="bg-yellow-50 rounded-lg p-6 max-w-2xl mx-auto">
          <h3 className="font-semibold text-lg mb-3">What you can do:</h3>
          <ul className="text-left space-y-2 text-sm text-gray-700">
            <li>🔄 Try booking again - the issue might be temporary</li>
            <li>📅 Choose a different time slot if available</li>
            <li>🌐 Check your internet connection</li>
            <li>💬 Contact our support team if the problem persists</li>
          </ul>
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col sm:flex-row gap-4 justify-center">
          <Button onClick={onRetry} size="lg">
            <RefreshCw className="w-4 h-4 mr-2" />
            Try Again
          </Button>
          <Button variant="outline" onClick={onContactSupport} size="lg">
            <MessageCircle className="w-4 h-4 mr-2" />
            Contact Support
          </Button>
        </div>

        {/* Additional Help */}
        <div className="text-center text-sm text-gray-500">
          <p>
            If you continue to experience issues, please contact our support
            team at{' '}
            <a
              href="mailto:support@example.com"
              className="text-blue-600 hover:underline"
            >
              support@example.com
            </a>
          </p>
        </div>
      </div>
    </div>
  );
}
