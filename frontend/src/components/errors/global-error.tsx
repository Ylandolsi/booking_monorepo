import { Button, Card } from '@/components/ui';
import { useGlobalErrorStore } from '@/stores/global-error-store';

export const GlobalErrorHandling = () => {
  const { error, setError, clearError } = useGlobalErrorStore();

  const simulateGlobalError = () => {
    setError(new Error('Global application error'));
  };

  const clearGlobalError = () => {
    clearError();
  };

  if (error) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-background p-4">
        <div className="max-w-md w-full text-center space-y-6">
          <div className="mx-auto w-16 h-16 bg-red-100 rounded-full flex items-center justify-center">
            <span className="text-2xl">⚠️</span>
          </div>

          <div className="space-y-2">
            <h1 className="text-2xl font-bold text-foreground">
              Application Error
            </h1>
            <p className="text-muted-foreground">{error.message}</p>
          </div>

          <div className="flex gap-3 justify-center">
            <Button onClick={clearGlobalError}>Try Again</Button>
            <Button variant="outline" onClick={() => window.location.reload()}>
              Reload Page
            </Button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">Global Error Handling</h3>

      <div className="space-y-4">
        <p className="text-muted-foreground">
          This demonstrates how to handle global application errors that affect
          the entire app.
        </p>

        <Button onClick={simulateGlobalError}>Simulate Global Error</Button>
      </div>
    </Card>
  );
};
