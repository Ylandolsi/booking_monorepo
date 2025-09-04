import { Button, DrawerDialog } from '@/components';
import { Check, X } from 'lucide-react';

interface BaseConfirmationDialogProps {
  isOpen: boolean;
  onClose: (e: boolean) => void;
  onConfirm: () => void;
  isLoading?: boolean;
  requestId: string;
  mentorName: string;
  amount: number;
}

export function ApprovePayoutDialog({
  isOpen,
  onClose,
  onConfirm,
  isLoading = false,
  requestId,
  mentorName,
  amount,
}: BaseConfirmationDialogProps) {
  return (
    <DrawerDialog
      open={isOpen}
      onOpenChange={onClose}
      description="                Confirm approval for this payout request
"
      title="Approve Payout Request"
    >
      <div className="space-y-4 py-4">
        <div className="space-y-2">
          <p className="text-sm text-muted-foreground">
            You are about to approve the payout request:
          </p>
          <div className="bg-muted p-4 rounded-lg space-y-2">
            <div className="flex justify-between">
              <span className="font-medium">Request ID:</span>
              <span className="font-mono text-sm">{requestId}</span>
            </div>
            <div className="flex justify-between">
              <span className="font-medium">Mentor:</span>
              <span>{mentorName}</span>
            </div>
            <div className="flex justify-between">
              <span className="font-medium">Amount:</span>
              <span className="font-semibold">${amount.toFixed(2)}</span>
            </div>
          </div>
        </div>

        <div className="bg-green-50 border border-green-200 rounded-lg p-3">
          <p className="text-sm text-green-800">
            <strong>Note:</strong> Once approved, this request will be processed
            for payment. This action cannot be undone.
          </p>
        </div>
      </div>

      <div className="gap-2 flex flex-row items-center">
        <Button
          variant="outline"
          onClick={() => onClose(false)}
          disabled={isLoading}
        >
          Cancel
        </Button>
        <Button
          onClick={onConfirm}
          disabled={isLoading}
          className="bg-green-600 hover:bg-green-700"
        >
          {isLoading ? (
            <>
              <div className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin mr-2" />
              Approving...
            </>
          ) : (
            <>
              <Check className="w-4 h-4 mr-2" />
              Approve Payout
            </>
          )}
        </Button>
      </div>
    </DrawerDialog>
  );
}

export function RejectPayoutDialog({
  isOpen,
  onClose,
  onConfirm,
  isLoading = false,
  requestId,
  mentorName,
  amount,
}: BaseConfirmationDialogProps) {
  return (
    <DrawerDialog
      open={isOpen}
      onOpenChange={onClose}
      description="Confirm rejection for this payout request"
      title="Reject Payout Request"
    >
      <div className="space-y-4 py-4">
        <div className="space-y-2">
          <p className="text-sm text-muted-foreground">
            You are about to reject the payout request:
          </p>
          <div className="bg-muted p-4 rounded-lg space-y-2">
            <div className="flex justify-between">
              <span className="font-medium">Request ID:</span>
              <span className="font-mono text-sm">{requestId}</span>
            </div>
            <div className="flex justify-between">
              <span className="font-medium">Mentor:</span>
              <span>{mentorName}</span>
            </div>
            <div className="flex justify-between">
              <span className="font-medium">Amount:</span>
              <span className="font-semibold">${amount.toFixed(2)}</span>
            </div>
          </div>
        </div>

        <div className="bg-red-50 border border-red-200 rounded-lg p-3">
          <p className="text-sm text-red-800">
            <strong>Warning:</strong> Once rejected, this request will be marked
            as denied. The mentor will be notified of the rejection.
          </p>
        </div>
      </div>
      <div className=" gap-2 flex flex-row items-center">
        <Button
          variant="outline"
          onClick={() => onClose(false)}
          disabled={isLoading}
        >
          Cancel
        </Button>
        <Button variant="destructive" onClick={onConfirm} disabled={isLoading}>
          {isLoading ? (
            <>
              <div className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin mr-2" />
              Rejecting...
            </>
          ) : (
            <>
              <X className="w-4 h-4 mr-2" />
              Reject Request
            </>
          )}
        </Button>
      </div>
    </DrawerDialog>
  );
}
