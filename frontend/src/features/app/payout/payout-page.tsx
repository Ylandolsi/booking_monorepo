import {
  Button,
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
  Badge,
  Alert,
  ErrorComponenet,
  LoadingState,
} from '@/components';
import { DrawerDialog } from '@/components/ui/drawer-dialog';
import { DollarSign, Calendar, TrendingUp, AlertCircle } from 'lucide-react';
import { PayoutRequestForm } from './components';
import { useHistoryPayout, useRequestPayout } from './api';
import { useState } from 'react';
import { useGetWallet } from '@/features/shared/get-wallet';
import { formatDate } from '@/utils/format';
import type { PayoutStatus } from '@/features/app/payout/types/payout';
import { useUser } from '@/features/auth';
import { useAppNavigation } from '@/hooks';

export function PayoutPage() {
  const [isPayoutDialogOpen, setIsPayoutDialogOpen] = useState(false);
  const [payoutSuccess, setPayoutSuccess] = useState(false);
  const requestPayoutMutation = useRequestPayout();
  const { data: walletData, error, isLoading } = useGetWallet();
  const {
    data: historyPayout,
    error: errorHistory,
    isLoading: isLoadingHistory,
  } = useHistoryPayout();
  const { data: user, isLoading: isUserLoading, error: userError } = useUser();
  const navigate = useAppNavigation();

  // Check if user is integrated with Konnect
  const isKonnectIntegrated = !!user?.konnectWalletId?.trim();

  if (isLoading || isLoadingHistory || isUserLoading) {
    return <LoadingState type="dots" />;
  }

  if (userError) {
    return (
      <ErrorComponenet
        message="Failed to load user data"
        title="Failed to load user"
      />
    );
  }

  if (!isKonnectIntegrated) {
    return (
      <ErrorComponenet
        title="Integration Required"
        message="You need to integrate with Konnect to access payout features. Please integrate with Konnect first."
        iconType="warning"
        showHomeButton={true}
        showBackButton={true}
      />
    );
  }

  // if (!isGoogleIntegrated) {
  //   return (
  //     <ErrorComponenet
  //       title="Google Integration Required"
  //       message="You need to integrate with Google Calendar to access payout features. Please integrate with Google Calendar first."
  //       iconType="warning"
  //       showHomeButton={true}
  //       showBackButton={true}
  //     />
  //   );
  // }

  if (error || walletData == null) {
    return (
      <ErrorComponenet
        message="Failed to load wallet"
        title="Failed to load wallet"
      />
    );
  }
  console.log(historyPayout, errorHistory);
  if (errorHistory) {
    return (
      <ErrorComponenet
        message="Failed to load payout history"
        title="Failed to load payout history"
      />
    );
  }
  if (isLoading || isLoadingHistory) {
    return <LoadingState type="dots" />;
  }

  const availableBalance = walletData?.balance;
  const pendingBalance = walletData?.pendingBalance;

  const handlePayoutRequest = async (amount: number) => {
    try {
      await requestPayoutMutation.mutateAsync(amount);
      setPayoutSuccess(true);
      setIsPayoutDialogOpen(false);

      // Reset success message after 5 seconds
      setTimeout(() => {
        setPayoutSuccess(false);
      }, 5000);
    } catch (error) {
      console.error('Payout request failed:', error);
    }
  };
  return (
    <div className="mx-auto p-6 space-y-8">
      {/* Header Section */}
      <div className="space-y-2">
        <h1 className="text-4xl font-bold tracking-light">Payouts</h1>
        <p className="text-muted-foreground text-lg">
          Manage your payout methods and view your transaction history
        </p>
      </div>

      {/* Success Alert */}
      {payoutSuccess && (
        <Alert className="bg-green-50 border-green-200 dark:bg-green-950/20 dark:border-green-900">
          <AlertCircle className="h-4 w-4 text-green-600 dark:text-green-400" />
          <div className="ml-2">
            <p className="text-sm text-green-800 dark:text-green-200">
              <strong>Payout request submitted successfully!</strong>
            </p>
            <p className="text-xs text-green-700 dark:text-green-300 mt-1">
              Your payout request is being processed and will be completed
              within 3-5 business days.
            </p>
          </div>
        </Alert>
      )}

      {/* Available Balance Card */}
      <Card className="bg-gradient-to-r from-white to-indigo-50/40 dark:from-blue-950/50 dark:to-indigo-950/50 border-ring dark:border-ring">
        <CardHeader className="pb-3">
          <CardTitle className="flex items-center gap-2text-primary">
            <DollarSign className="h-5 w-5" />
            Available for Payout
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="flex flex-col sm:flex-row gap-5 sm:gap-0 sm:items-center justify-between">
            <div className="space-y-1">
              <div className="text-4xl font-bold text-primary">
                ${availableBalance.toFixed(2)}
              </div>
              <div className="text-sm text-muted-foreground flex items-center gap-1">
                <TrendingUp className="h-4 w-4" />
                Ready to withdraw
              </div>
            </div>
            <DrawerDialog
              open={isPayoutDialogOpen}
              onOpenChange={setIsPayoutDialogOpen}
              trigger={
                <Button
                  size="lg"
                  className="bg-primary hover:bg-primary/80"
                  disabled={availableBalance < 20}
                >
                  Request Payout
                </Button>
              }
              title="Request Payout"
              description="Withdraw funds from your available balance"
            >
              <PayoutRequestForm
                availableBalance={availableBalance}
                onSubmit={handlePayoutRequest}
                isLoading={requestPayoutMutation.isPending}
                onCancel={() => setIsPayoutDialogOpen(false)}
              />
            </DrawerDialog>
          </div>
        </CardContent>
      </Card>

      {/* Low Balance Notice */}
      {availableBalance < 20 && (
        <Alert className="bg-yellow-50 border-yellow-200 dark:bg-yellow-950/20 dark:border-yellow-900">
          <AlertCircle className="h-4 w-4 text-yellow-600 dark:text-yellow-400" />
          <div className="ml-2">
            <p className="text-sm text-yellow-800 dark:text-yellow-200">
              <strong>Minimum payout requirement:</strong> You need at least
              $20.00 to request a payout.
            </p>
            <p className="text-xs text-yellow-700 dark:text-yellow-300 mt-1">
              Continue earning through mentoring sessions to reach the minimum
              threshold.
            </p>
          </div>
        </Alert>
      )}

      {/* Payout History Section */}
      <div className="space-y-4">
        <div className="flex items-center gap-2">
          <Calendar className="h-5 w-5 text-muted-foreground" />
          <h2 className="text-2xl font-semibold">Payout History</h2>
        </div>
        <Card>
          <CardHeader>
            <CardTitle>Recent Transactions</CardTitle>
            <CardDescription>
              Your complete payout transaction history
            </CardDescription>
          </CardHeader>
          <CardContent>
            <Table>
              <TableCaption className="text-left">
                A list of your recent payout transactions.
              </TableCaption>
              <TableHeader>
                <TableRow>
                  <TableHead>Date</TableHead>
                  <TableHead>Amount</TableHead>
                  <TableHead>Status</TableHead>
                  <TableHead className="text-right">Payment Ref</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {historyPayout?.map((ph) => (
                  <TableRow key={ph.id}>
                    <TableCell className="font-medium">
                      {formatDate(ph.createdAt)}
                    </TableCell>
                    <TableCell className="font-semibold">
                      ${ph.amount.toFixed(2)}
                    </TableCell>
                    <TableCell>
                      <Badge {...getStatusBadgeProps(ph.status)}>
                        {ph.status}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-right text-muted-foreground font-mono text-sm">
                      {ph.paymentRef == ''
                        ? 'not available yet'
                        : ph.paymentRef}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </CardContent>
        </Card>
      </div>

      {/* Quick Stats Cards */}
      {/* <div className="grid grid-cols-1 md:grid-cols-3 gap-4"> */}
      <div className="grid grid-cols-1  gap-4">
        {/* <Card>
          <CardHeader className="pb-2">
            <CardDescription>Total Earned</CardDescription>
            <CardTitle className="text-2xl">$12,845.67</CardTitle>
          </CardHeader>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>This Month</CardDescription>
            <CardTitle className="text-2xl">$2,222.00</CardTitle>
          </CardHeader>
        </Card> */}
        <Card className="bg-gradient-to-r from-yellow-50 to-white border border-yellow-600">
          <CardHeader className="pb-2">
            <CardDescription>Pending</CardDescription>
            <CardTitle className="text-2xl">${pendingBalance}</CardTitle>
          </CardHeader>
        </Card>
      </div>
    </div>
  );
}

const getStatusBadgeProps = (status: PayoutStatus) => {
  switch (status) {
    case 'Pending':
      return {
        variant: 'secondary' as const,
        className:
          'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-100',
      };
    case 'Approved':
      return {
        variant: 'default' as const,
        className:
          'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-100',
      };

    case 'Completed':
      return {
        variant: 'default' as const,
        className:
          'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-100',
      };
    case 'Rejected':
      return {
        variant: 'destructive' as const,
        className: 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-100',
      };
    default:
      return {
        variant: 'default' as const,
        className: '',
      };

    // case 'processing':
    //   return {
    //     variant: 'secondary' as const,
    //     className:
    //       'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-100',
    //   };
  }
};
