import { useState, useMemo } from 'react';
import { useAppNavigation, useTimeFilter } from '@/hooks';
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
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
  LoadingState,
  ErrorComponenet,
} from '@/components';
import { Filter, Eye, Check, X } from 'lucide-react';
import { ApprovePayoutDialog, RejectPayoutDialog } from './components';
import { useGetAllPayoutsAdmin, useApprovePayoutAdmin, useRejectPayoutAdmin, type AdminPayoutResponse } from './api';
import { type PayoutStatus, type TimeFilter, mapPayoutStatus } from './types/admin-payout';
import { formatDate } from '@/utils/format';

export function AdminPayoutRequestsPage() {
  const nav = useAppNavigation();
  const [statusFilter, setStatusFilter] = useState<PayoutStatus | 'all'>('all');

  const { upToDate, timeFilter, setTimeStatus } = useTimeFilter();

  // Dialog states
  const [isApproveDialogOpen, setIsApproveDialogOpen] = useState(false);
  const [isRejectDialogOpen, setIsRejectDialogOpen] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState<AdminPayoutResponse | null>(null);

  // API hooks
  const { data: payoutRequests, error, isLoading } = useGetAllPayoutsAdmin(statusFilter === 'all' ? undefined : statusFilter, upToDate, undefined);

  const approvePayoutMutation = useApprovePayoutAdmin();
  const rejectPayoutMutation = useRejectPayoutAdmin();

  const getStatusBadgeProps = (status: PayoutStatus) => {
    switch (status) {
      case 'pending':
        return {
          variant: 'secondary' as const,
          className: 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-100',
        };
      case 'approved':
        return {
          variant: 'default' as const,
          className: 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-100',
        };
      case 'completed':
        return {
          variant: 'default' as const,
          className: 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-100',
        };
      case 'rejected':
        return {
          variant: 'destructive' as const,
          className: 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-100',
        };
      default:
        return {
          variant: 'default' as const,
          className: '',
        };
    }
  };

  // // Calculate statistics
  // const stats = useMemo(() => {
  //   const pendingRequests = filteredRequests.filter(
  //     (req) => mapPayoutStatus(req.status) === 'pending',
  //   );
  //   const totalAmount = filteredRequests.reduce(
  //     (sum, req) => sum + req.amount,
  //     0,
  //   );
  //   const pendingAmount = pendingRequests.reduce(
  //     (sum, req) => sum + req.amount,
  //     0,
  //   );

  //   return {
  //     totalRequests: filteredRequests.length,
  //     pendingRequests: pendingRequests.length,
  //     totalAmount,
  //     pendingAmount,
  //   };
  // }, [filteredRequests]);

  // Loading and error states
  if (isLoading) {
    return <LoadingState type="dots" />;
  }

  if (error || !payoutRequests) {
    return <ErrorComponenet message="Failed to load payout requests" title="Error Loading Data" />;
  }

  const handleApprove = async (request: AdminPayoutResponse) => {
    setSelectedRequest(request);
    setIsApproveDialogOpen(true);
  };

  const handleReject = async (request: AdminPayoutResponse) => {
    setSelectedRequest(request);
    setIsRejectDialogOpen(true);
  };

  const handleView = (requestId: number) => {
    nav.goToAdminPayoutRequestDetails(requestId.toString());
  };

  const confirmApprove = async () => {
    if (!selectedRequest) return;

    try {
      const result = await approvePayoutMutation.mutateAsync(selectedRequest.id);
      console.log('Payout approved, PayUrl:', result.payUrl);
      setIsApproveDialogOpen(false);
      setSelectedRequest(null);
      // The query will be invalidated automatically via meta.invalidatesQuery
    } catch (error) {
      console.error('Error approving request:', error);
    }
  };

  const confirmReject = async () => {
    if (!selectedRequest) return;

    try {
      await rejectPayoutMutation.mutateAsync(selectedRequest.id);
      setIsRejectDialogOpen(false);
      setSelectedRequest(null);
      // The query will be invalidated automatically via meta.invalidatesQuery
    } catch (error) {
      console.error('Error rejecting request:', error);
    }
  };

  return (
    <div className="mx-auto p-6 space-y-8">
      {/* Header Section */}
      <div className="space-y-2">
        <h1 className="text-4xl font-bold tracking-light">Payout Requests Management</h1>
        <p className="text-muted-foreground text-lg">Review and manage mentor payout requests</p>
      </div>

      {/* Stats Cards */}
      {/* <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Total Requests</CardDescription>
             <CardTitle className="text-2xl">{stats.totalRequests}</CardTitle>
          </CardHeader>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Pending Reviews</CardDescription>
            <CardTitle className="text-2xl text-yellow-600">
              {stats.pendingRequests}
            </CardTitle>
          </CardHeader>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Total Amount</CardDescription>
            <CardTitle className="text-2xl">
              ${stats.totalAmount.toFixed(2)}
            </CardTitle>
          </CardHeader>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Pending Amount</CardDescription>
            <CardTitle className="text-2xl text-yellow-600">
              ${stats.pendingAmount.toFixed(2)}
            </CardTitle>
          </CardHeader>
        </Card>
      </div> */}

      {/* Filters Section */}
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <Filter className="h-5 w-5 text-muted-foreground" />
              <CardTitle>Filters</CardTitle>
            </div>
          </div>
        </CardHeader>
        <CardContent>
          <div className="flex flex-col sm:flex-row gap-4">
            <div className="space-y-2">
              <label className="text-sm font-medium">Time Period</label>
              <Select value={timeFilter} onValueChange={(value: TimeFilter) => setTimeStatus(value)}>
                <SelectTrigger className="w-[180px]">
                  <SelectValue placeholder="Select time period" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="All">All Time</SelectItem>
                  <SelectItem value="LastHour">Last Hour</SelectItem>
                  <SelectItem value="Last24Hours">Last 24 Hours </SelectItem>
                  <SelectItem value="Last3Days">Last 3 Days</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium">Status</label>
              <Select value={statusFilter} onValueChange={(value: PayoutStatus | 'all') => setStatusFilter(value)}>
                <SelectTrigger className="w-[180px]">
                  <SelectValue placeholder="Select status" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Statuses</SelectItem>
                  <SelectItem value="pending">Pending</SelectItem>
                  <SelectItem value="approved">Approved</SelectItem>
                  <SelectItem value="completed">Completed</SelectItem>
                  <SelectItem value="rejected">Rejected</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Payout Requests Table */}
      <Card>
        <CardHeader>
          <CardTitle>Payout Requests</CardTitle>
          <CardDescription>{/* {filteredRequests.length} request(s) found */}</CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableCaption className="text-left">Payout requests from mentors awaiting review.</TableCaption>
            <TableHeader>
              <TableRow>
                <TableHead>Request ID</TableHead>
                <TableHead>Mentor</TableHead>
                <TableHead>Amount</TableHead>
                <TableHead>Method</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Request Date</TableHead>
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {payoutRequests.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={7} className="text-center py-8 text-muted-foreground">
                    No payout requests found for the selected filters.
                  </TableCell>
                </TableRow>
              ) : (
                payoutRequests.map((request) => {
                  const status = mapPayoutStatus(request.status);
                  const statusProps = getStatusBadgeProps(status);
                  return (
                    <TableRow key={request.id}>
                      <TableCell className="font-medium font-mono text-sm">PR-{request.id.toString().padStart(3, '0')}</TableCell>
                      <TableCell>
                        <div className="space-y-1">
                          <div className="font-medium">User ID: {request.userId}</div>
                          <div className="text-sm text-muted-foreground">Wallet: {request.konnectWalletId}</div>
                        </div>
                      </TableCell>
                      <TableCell className="font-semibold">${request.amount.toFixed(2)}</TableCell>
                      <TableCell>Konnect Wallet</TableCell>
                      <TableCell>
                        <Badge {...statusProps}>{status.charAt(0).toUpperCase() + status.slice(1)}</Badge>
                      </TableCell>
                      <TableCell className="text-sm">{formatDate(request.createdAt)}</TableCell>
                      <TableCell className="text-right">
                        <div className="flex items-center justify-end gap-2">
                          <Button variant="ghost" size="sm" onClick={() => handleView(request.id)} className="h-8 w-8 p-0">
                            <Eye className="h-4 w-4" />
                          </Button>
                          {status === 'pending' && (
                            <>
                              <Button
                                variant="ghost"
                                size="sm"
                                onClick={() => handleApprove(request)}
                                className="h-8 w-8 p-0 text-green-600 hover:text-green-700 hover:bg-green-50"
                                disabled={approvePayoutMutation.isPending}
                              >
                                <Check className="h-4 w-4" />
                              </Button>
                              <Button
                                variant="ghost"
                                size="sm"
                                onClick={() => handleReject(request)}
                                className="h-8 w-8 p-0 text-red-600 hover:text-red-700 hover:bg-red-50"
                                disabled={rejectPayoutMutation.isPending}
                              >
                                <X className="h-4 w-4" />
                              </Button>
                            </>
                          )}
                        </div>
                      </TableCell>
                    </TableRow>
                  );
                })
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      {/* Confirmation Dialogs */}
      {selectedRequest && (
        <>
          <ApprovePayoutDialog
            isOpen={isApproveDialogOpen}
            onClose={(e: boolean) => {
              setIsApproveDialogOpen(e);
              setSelectedRequest(null);
            }}
            onConfirm={confirmApprove}
            isLoading={approvePayoutMutation.isPending}
            requestId={`PR-${selectedRequest.id.toString().padStart(3, '0')}`}
            mentorName={`User ID: ${selectedRequest.userId}`}
            amount={selectedRequest.amount}
          />

          <RejectPayoutDialog
            isOpen={isRejectDialogOpen}
            onClose={(e: boolean) => {
              setIsRejectDialogOpen(e);
              setSelectedRequest(null);
            }}
            onConfirm={confirmReject}
            isLoading={rejectPayoutMutation.isPending}
            requestId={`PR-${selectedRequest.id.toString().padStart(3, '0')}`}
            mentorName={`User ID: ${selectedRequest.userId}`}
            amount={selectedRequest.amount}
          />
        </>
      )}
    </div>
  );
}
