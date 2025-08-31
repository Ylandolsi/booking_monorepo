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

// Mock data - this would come from an API call based on the request ID
const mockPayoutRequest = {
  id: 'PR-001',
  mentorName: 'Ahmed Ben Ali',
  mentorEmail: 'ahmed.benali@example.com',
  amount: 1234.56,
  method: 'Bank Transfer',
  status: 'pending' as const,
  requestDate: '2024-08-30T10:30:00Z',
  mentorId: 'M001',
  details: {
    sessionCount: 12,
    totalEarnings: 1500.00,
    platformFee: 265.44,
    netAmount: 1234.56,
    accountDetails: {
      bankName: 'Bank ABC',
      accountNumber: '****1234',
      routingNumber: '021000021',
      accountHolder: 'Ahmed Ben Ali',
    },
    sessionHistory: [
      { date: '2024-08-25', student: 'John Doe', duration: 60, amount: 125.00 },
      { date: '2024-08-23', student: 'Jane Smith', duration: 45, amount: 93.75 },
      { date: '2024-08-22', student: 'Mike Johnson', duration: 60, amount: 125.00 },
      { date: '2024-08-20', student: 'Sarah Wilson', duration: 30, amount: 62.50 },
    ],
    notes: 'Regular monthly payout request. All sessions completed successfully.',
  },
};

type PayoutStatus = 'pending' | 'approved' | 'processing' | 'completed' | 'rejected';

const getStatusBadgeProps = (status: PayoutStatus) => {
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
    case 'processing':
      return {
        variant: 'secondary' as const,
        className: 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-100',
        icon: <AlertCircle className="w-4 h-4" />,
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
};

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};

export function AdminPayoutRequestDetailsPage() {
  const navigate = useNavigate();
  const params = useParams({ from: '/app/admin/payout-requests/$requestId' });
  const requestId = params.requestId;
  
  const [isApproveDialogOpen, setIsApproveDialogOpen] = useState(false);
  const [isRejectDialogOpen, setIsRejectDialogOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  // In a real app, you'd fetch the payout request data based on requestId
  const payoutRequest = { ...mockPayoutRequest, id: requestId };
  const statusProps = getStatusBadgeProps(payoutRequest.status);

  const handleApprove = async () => {
    setIsLoading(true);
    try {
      // TODO: Implement actual approve API call
      console.log('Approving request:', requestId);
      await new Promise(resolve => setTimeout(resolve, 2000)); // Simulate API call
      setIsApproveDialogOpen(false);
      // Update local state or refresh data
    } catch (error) {
      console.error('Error approving request:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleReject = async () => {
    setIsLoading(true);
    try {
      // TODO: Implement actual reject API call
      console.log('Rejecting request:', requestId);
      await new Promise(resolve => setTimeout(resolve, 2000)); // Simulate API call
      setIsRejectDialogOpen(false);
      // Update local state or refresh data
    } catch (error) {
      console.error('Error rejecting request:', error);
    } finally {
      setIsLoading(false);
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
              {payoutRequest.status.charAt(0).toUpperCase() + payoutRequest.status.slice(1)}
            </Badge>
          </div>
          <p className="text-muted-foreground">
            Request ID: <span className="font-mono font-medium">{payoutRequest.id}</span>
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
                      Mentor Information
                    </div>
                    <div>
                      <p className="font-medium">{payoutRequest.mentorName}</p>
                      <p className="text-sm text-muted-foreground">{payoutRequest.mentorEmail}</p>
                      <p className="text-sm text-muted-foreground">ID: {payoutRequest.mentorId}</p>
                    </div>
                  </div>
                  
                  <div className="space-y-2">
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <Calendar className="w-4 h-4" />
                      Request Date
                    </div>
                    <p className="font-medium">{formatDate(payoutRequest.requestDate)}</p>
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
                    <p className="font-medium">{payoutRequest.method}</p>
                  </div>
                </div>

                {payoutRequest.details.notes && (
                  <>
                    <Separator />
                    <div className="space-y-2">
                      <h4 className="font-medium">Notes</h4>
                      <p className="text-sm text-muted-foreground">{payoutRequest.details.notes}</p>
                    </div>
                  </>
                )}
              </CardContent>
            </Card>

            {/* Financial Breakdown */}
            <Card>
              <CardHeader>
                <CardTitle>Financial Breakdown</CardTitle>
                <CardDescription>
                  Detailed breakdown of earnings and fees
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="space-y-3">
                  <div className="flex justify-between">
                    <span>Total Earnings ({payoutRequest.details.sessionCount} sessions)</span>
                    <span className="font-medium">${payoutRequest.details.totalEarnings.toFixed(2)}</span>
                  </div>
                  <div className="flex justify-between text-red-600">
                    <span>Platform Fee (17.7%)</span>
                    <span>-${payoutRequest.details.platformFee.toFixed(2)}</span>
                  </div>
                  <Separator />
                  <div className="flex justify-between text-lg font-semibold">
                    <span>Net Amount</span>
                    <span>${payoutRequest.details.netAmount.toFixed(2)}</span>
                  </div>
                </div>
              </CardContent>
            </Card>

            {/* Recent Sessions */}
            <Card>
              <CardHeader>
                <CardTitle>Recent Sessions</CardTitle>
                <CardDescription>
                  Sessions included in this payout request
                </CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-3">
                  {payoutRequest.details.sessionHistory.map((session, index) => (
                    <div key={index} className="flex items-center justify-between p-3 bg-muted rounded-lg">
                      <div>
                        <p className="font-medium">{session.student}</p>
                        <p className="text-sm text-muted-foreground">
                          {new Date(session.date).toLocaleDateString()} • {session.duration} min
                        </p>
                      </div>
                      <span className="font-medium">${session.amount.toFixed(2)}</span>
                    </div>
                  ))}
                </div>
              </CardContent>
            </Card>
          </div>

          {/* Sidebar */}
          <div className="space-y-6">
            {/* Quick Actions */}
            {payoutRequest.status === 'pending' && (
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
                  >
                    <CheckCircle className="w-4 h-4 mr-2" />
                    Approve Request
                  </Button>
                  <Button 
                    variant="destructive"
                    onClick={() => setIsRejectDialogOpen(true)}
                    className="w-full"
                  >
                    <XCircle className="w-4 h-4 mr-2" />
                    Reject Request
                  </Button>
                </CardContent>
              </Card>
            )}

            {/* Account Details */}
            <Card>
              <CardHeader>
                <CardTitle>Account Details</CardTitle>
                <CardDescription>
                  Payout destination information
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-3">
                <div>
                  <label className="text-sm font-medium text-muted-foreground">Bank Name</label>
                  <p>{payoutRequest.details.accountDetails.bankName}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-muted-foreground">Account Holder</label>
                  <p>{payoutRequest.details.accountDetails.accountHolder}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-muted-foreground">Account Number</label>
                  <p className="font-mono">{payoutRequest.details.accountDetails.accountNumber}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-muted-foreground">Routing Number</label>
                  <p className="font-mono">{payoutRequest.details.accountDetails.routingNumber}</p>
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
        isLoading={isLoading}
        requestId={payoutRequest.id}
        mentorName={payoutRequest.mentorName}
        amount={payoutRequest.amount}
      />

      <RejectPayoutDialog
        isOpen={isRejectDialogOpen}
        onClose={() => setIsRejectDialogOpen(false)}
        onConfirm={handleReject}
        isLoading={isLoading}
        requestId={payoutRequest.id}
        mentorName={payoutRequest.mentorName}
        amount={payoutRequest.amount}
      />
    </>
  );
}