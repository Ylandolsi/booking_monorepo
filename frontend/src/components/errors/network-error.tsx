import { Button } from '@/components/ui';
import { WifiOff, RefreshCw, Wifi } from 'lucide-react';

interface NetworkErrorProps {
  onRetry?: () => void;
  customMessage?: string;
}

export const NetworkError = ({ onRetry, customMessage }: NetworkErrorProps) => {
  const handleRetry = () => {
    if (onRetry) {
      onRetry();
    } else {
      window.location.reload();
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-background p-4">
      <div className="max-w-md w-full text-center space-y-6">
        {/* Network Error Icon */}
        <div className="mx-auto w-16 h-16 bg-orange-100 dark:bg-orange-900/20 rounded-full flex items-center justify-center">
          <WifiOff className="w-8 h-8 text-orange-600 dark:text-orange-400" />
        </div>

        {/* Error Message */}
        <div className="space-y-2">
          <h1 className="text-2xl font-bold text-foreground">
            Network Connection Error
          </h1>
          <p className="text-muted-foreground">
            {customMessage ||
              "It looks like you're having trouble connecting to our servers. Please check your internet connection and try again."}
          </p>
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col sm:flex-row gap-3 justify-center">
          <Button onClick={handleRetry} className="flex items-center gap-2">
            <RefreshCw className="w-4 h-4" />
            Try Again
          </Button>

          <Button
            variant="outline"
            onClick={() => window.location.reload()}
            className="flex items-center gap-2"
          >
            <Wifi className="w-4 h-4" />
            Check Connection
          </Button>
        </div>

        {/* Troubleshooting Tips */}
        <div className="text-xs text-muted-foreground space-y-1">
          <p className="font-medium">Troubleshooting tips:</p>
          <ul className="text-left space-y-1">
            <li>• Check your internet connection</li>
            <li>• Try refreshing the page</li>
            <li>• Disable VPN if you're using one</li>
            <li>• Clear your browser cache</li>
          </ul>
        </div>
      </div>
    </div>
  );
};
