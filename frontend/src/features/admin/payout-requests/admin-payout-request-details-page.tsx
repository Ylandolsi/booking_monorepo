import { useState } from 'react';
import { useNavigate, useParams } from '@tanstack/react-router';
import {
  Button,
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
  Badge,
  Separator,
  LoadingState,
  ErrorComponenet,
} from '@/components';
import {
  ArrowLeft,
  Calendar,
  DollarSign,
  User,
  CreditCard,
  Clock,
  CheckCircle,
  XCircle,
  AlertCircle,
  Eye,
} from 'lucide-react';
import { ApprovePayoutDialog, RejectPayoutDialog } from './components';
import {
  useGetAllPayoutsAdmin,
  useApprovePayoutAdmin,
  useRejectPayoutAdmin,
} from './api';
import { formatDate } from '@/utils/format';
import { mapPayoutStatus, type PayoutStatus } from './types/admin-payout';

export function AdminPayoutRequestDetailsPage() {
  const navigate = useNavigate();
  const params = useParams({ from: '/app/admin/payout-requests/$requestId' });
  const requestId = parseInt(params.requestId, 10);

  const [isApproveDialogOpen, setIsApproveDialogOpen] = useState(false);
  const [isRejectDialogOpen, setIsRejectDialogOpen] = useState(false);

  // API hooks
  const { data: allPayouts, error, isLoading } = useGetAllPayoutsAdmin();
  const approvePayoutMutation = useApprovePayoutAdmin();
  const rejectPayoutMutation = useRejectPayoutAdmin();

  // Loading and error states
  if (isLoading) {
    return <LoadingState type="dots" />;
  }

  if (error || !allPayouts) {
    return (
      <ErrorComponenet
        message="Failed to load payout request"
        title="Error Loading Data"
      />
    );
  }

  // Find the specific payout request
  const payoutRequest = allPayouts.find(p => p.id === requestId);
  
  if (!payoutRequest) {
    return (
      <ErrorComponenet
        message="Payout request not found"
        title="Request Not Found"
      />
    );
  }

  const status = mapPayoutStatus(payoutRequest.status);
  const statusProps = getStatusBadgeProps(status);

  // Helper function for status badge props
  function getStatusBadgeProps(status: PayoutStatus) {
    switch (status) {
      case 'pending':
        return {
          variant: 'secondary' as const,
          className: 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-100',
          icon: <Clock className="w-4 h-4" />,
        };
      case 'approved':
        return {
          variant: 'default' as const,
          className: 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-100',
          icon: <CheckCircle className="w-4 h-4" />,
        };
      case 'completed':
        return {
          variant: 'default' as const,
          className: 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-100',
          icon: <CheckCircle className="w-4 h-4" />,
        };
      case 'rejected':
        return {
          variant: 'destructive' as const,
          className: 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-100',
          icon: <XCircle className="w-4 h-4" />,
        };
      default:
        return {
          variant: 'default' as const,
          className: '',
          icon: <Clock className="w-4 h-4" />,
        };
    }
  }

  const handleApprove = async () => {
    try {
      const result = await approvePayoutMutation.mutateAsync(payoutRequest.id);
      console.log('Payout approved, PayUrl:', result.payUrl);
      setIsApproveDialogOpen(false);
      // The query will be invalidated automatically and page will reflect changes
    } catch (error) {
      console.error('Error approving request:', error);
    }
  };

  const handleReject = async () => {
    try {
      await rejectPayoutMutation.mutateAsync(payoutRequest.id);
      setIsRejectDialogOpen(false);
      // The query will be invalidated automatically and page will reflect changes
    } catch (error) {
      console.error('Error rejecting request:', error);
    }
  };

  const handleBack = () => {
    navigate({ to: '/app/admin/payout-requests' });
  };

  return (
    <>
      <div className="mx-auto p-6 space-y-6">
        {/* Header with Back Button */}
        <div className="flex items-center gap-4">
          <Button 
            variant="ghost" 
            size="sm" 
            onClick={handleBack}
            className="gap-2"
          >
            <ArrowLeft className="w-4 h-4" />
            Back to Payout Requests
          </Button>
        </div>

        {/* Page Header */}
        <div className="space-y-2">
          <div className="flex items-center justify-between">
            <h1 className="text-3xl font-bold tracking-tight">Payout Request Details</h1>
            <Badge {...statusProps} className="gap-2">
              {statusProps.icon}
              {status.charAt(0).toUpperCase() + status.slice(1)}
            </Badge>
          </div>
          <p className="text-muted-foreground">
            Request ID: <span className="font-mono font-medium">PR-{payoutRequest.id.toString().padStart(3, '0')}</span>
          </p>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Main Details */}
          <div className="lg:col-span-2 space-y-6">
            {/* Request Overview */}
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <Eye className="w-5 h-5" />
                  Request Overview
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <User className="w-4 h-4" />
                      User Information
                    </div>
                    <div>
                      <p className="font-medium">User ID: {payoutRequest.userId}</p>
                      <p className="text-sm text-muted-foreground">Konnect Wallet: {payoutRequest.konnectWalletId}</p>
                      <p className="text-sm text-muted-foreground">Wallet ID: {payoutRequest.walletId}</p>
                    </div>
                  </div>
                  
                  <div className="space-y-2">
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <Calendar className="w-4 h-4" />
                      Request Date
                    </div>
                    <p className="font-medium">{formatDate(payoutRequest.createdAt)}</p>
                  </div>

                  <div className="space-y-2">
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <DollarSign className="w-4 h-4" />
                      Amount Requested
                    </div>
                    <p className="text-2xl font-bold">${payoutRequest.amount.toFixed(2)}</p>
                  </div>

                  <div className="space-y-2">
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <CreditCard className="w-4 h-4" />
                      Payment Method
                    </div>
                    <p className="font-medium">Konnect Wallet</p>
                  </div>
                </div>

                {payoutRequest.paymentRef && payoutRequest.paymentRef !== '' && (
                  <>
                    <Separator />
                    <div className="space-y-2">
                      <h4 className="font-medium">Payment Reference</h4>
                      <p className="text-sm text-muted-foreground font-mono">{payoutRequest.paymentRef}</p>
                    </div>
                  </>
                )}
              </CardContent>
            </Card>

            {/* Financial Breakdown */}
            <Card>
              <CardHeader>
                <CardTitle>Financial Information</CardTitle>
                <CardDescription>
                  Basic payout information
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="space-y-3">
                  <div className="flex justify-between">
                    <span>Requested Amount</span>
                    <span className="font-medium">${payoutRequest.amount.toFixed(2)}</span>
                  </div>
                  <div className="flex justify-between">
                    <span>Status</span>
                    <Badge {...statusProps}>{status}</Badge>
                  </div>
                  <div className="flex justify-between">
                    <span>Created At</span>
                    <span className="text-sm">{formatDate(payoutRequest.createdAt)}</span>
                  </div>
                  <div className="flex justify-between">
                    <span>Last Updated</span>
                    <span className="text-sm">{formatDate(payoutRequest.updatedAt)}</span>
                  </div>
                </div>
              </CardContent>
            </Card>

            {/* System Information */}
            <Card>
              <CardHeader>
                <CardTitle>System Information</CardTitle>
                <CardDescription>
                  Internal tracking details
                </CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-3">
                  <div className="flex justify-between p-3 bg-muted rounded-lg">
                    <div>
                      <p className="font-medium">Payout ID</p>
                      <p className="text-sm text-muted-foreground">Internal system identifier</p>
                    </div>
                    <span className="font-mono">{payoutRequest.id}</span>
                  </div>
                  <div className="flex justify-between p-3 bg-muted rounded-lg">
                    <div>
                      <p className="font-medium">User ID</p>
                      <p className="text-sm text-muted-foreground">Requesting user</p>
                    </div>
                    <span className="font-mono">{payoutRequest.userId}</span>
                  </div>
                  <div className="flex justify-between p-3 bg-muted rounded-lg">
                    <div>
                      <p className="font-medium">Wallet ID</p>
                      <p className="text-sm text-muted-foreground">Internal wallet reference</p>
                    </div>
                    <span className="font-mono">{payoutRequest.walletId}</span>
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>

          {/* Sidebar */}
          <div className="space-y-6">
            {/* Quick Actions */}
            {status === 'pending' && (
              <Card>
                <CardHeader>
                  <CardTitle>Quick Actions</CardTitle>
                  <CardDescription>
                    Approve or reject this payout request
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-3">
                  <Button 
                    onClick={() => setIsApproveDialogOpen(true)}
                    className="w-full bg-green-600 hover:bg-green-700"
                    disabled={approvePayoutMutation.isPending || rejectPayoutMutation.isPending}
                  >
                    <CheckCircle className="w-4 h-4 mr-2" />
                    {approvePayoutMutation.isPending ? 'Approving...' : 'Approve Request'}
                  </Button>
                  <Button 
                    variant="destructive"
                    onClick={() => setIsRejectDialogOpen(true)}
                    className="w-full"
                    disabled={approvePayoutMutation.isPending || rejectPayoutMutation.isPending}
                  >
                    <XCircle className="w-4 h-4 mr-2" />
                    {rejectPayoutMutation.isPending ? 'Rejecting...' : 'Reject Request'}
                  </Button>
                </CardContent>
              </Card>
            )}

            {/* Payout Details */}
            <Card>
              <CardHeader>
                <CardTitle>Payout Details</CardTitle>
                <CardDescription>
                  Konnect wallet information
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-3">
                <div>
                  <label className="text-sm font-medium text-muted-foreground">Konnect Wallet ID</label>
                  <p className="font-mono text-sm">{payoutRequest.konnectWalletId}</p>
                </div>
                {payoutRequest.paymentRef && payoutRequest.paymentRef !== '' && (
                  <div>
                    <label className="text-sm font-medium text-muted-foreground">Payment Reference</label>
                    <p className="font-mono text-sm">{payoutRequest.paymentRef}</p>
                  </div>
                )}
                <div>
                  <label className="text-sm font-medium text-muted-foreground">Status</label>
                  <div className="mt-1">
                    <Badge {...statusProps}>
                      {statusProps.icon}
                      <span className="ml-1">{status}</span>
                    </Badge>
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>
        </div>
      </div>

      {/* Confirmation Dialogs */}
      <ApprovePayoutDialog
        isOpen={isApproveDialogOpen}
        onClose={() => setIsApproveDialogOpen(false)}
        onConfirm={handleApprove}
        isLoading={approvePayoutMutation.isPending}
        requestId={`PR-${payoutRequest.id.toString().padStart(3, '0')}`}
        mentorName={`User ID: ${payoutRequest.userId}`}
        amount={payoutRequest.amount}
      />

      <RejectPayoutDialog
        isOpen={isRejectDialogOpen}
        onClose={() => setIsRejectDialogOpen(false)}
        onConfirm={handleReject}
        isLoading={rejectPayoutMutation.isPending}
        requestId={`PR-${payoutRequest.id.toString().padStart(3, '0')}`}
        mentorName={`User ID: ${payoutRequest.userId}`}
        amount={payoutRequest.amount}
      />
    </>
  );
}