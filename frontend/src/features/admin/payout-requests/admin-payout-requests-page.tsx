import { useState, useMemo } from 'react';
import { useAppNavigation } from '@/hooks';
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
} from '@/components';
import {
  Filter,
  Eye,
  Check,
  X,
} from 'lucide-react';
import { ApprovePayoutDialog, RejectPayoutDialog } from './components';

// Mock data for payout requests
const mockPayoutRequests: Array<{
  id: string;
  mentorName: string;
  mentorEmail: string;
  amount: number;
  method: string;
  status: PayoutStatus;
  requestDate: string;
  mentorId: string;
}> = [
  {
    id: 'PR-001',
    mentorName: 'Ahmed Ben Ali',
    mentorEmail: 'ahmed.benali@example.com',
    amount: 1234.56,
    method: 'Bank Transfer',
    status: 'pending',
    requestDate: '2024-08-30T10:30:00Z',
    mentorId: 'M001',
  },
  {
    id: 'PR-002',
    mentorName: 'Sarah Johnson',
    mentorEmail: 'sarah.johnson@example.com',
    amount: 892.30,
    method: 'PayPal',
    status: 'approved',
    requestDate: '2024-08-29T14:15:00Z',
    mentorId: 'M002',
  },
  {
    id: 'PR-003',
    mentorName: 'Mohamed Trabelsi',
    mentorEmail: 'mohamed.trabelsi@example.com',
    amount: 567.89,
    method: 'Bank Transfer',
    status: 'processing',
    requestDate: '2024-08-29T09:45:00Z',
    mentorId: 'M003',
  },
  {
    id: 'PR-004',
    mentorName: 'Emily Davis',
    mentorEmail: 'emily.davis@example.com',
    amount: 2100.75,
    method: 'Stripe',
    status: 'completed',
    requestDate: '2024-08-28T16:20:00Z',
    mentorId: 'M004',
  },
  {
    id: 'PR-005',
    mentorName: 'Karim Sassi',
    mentorEmail: 'karim.sassi@example.com',
    amount: 445.20,
    method: 'Bank Transfer',
    status: 'rejected',
    requestDate: '2024-08-28T11:30:00Z',
    mentorId: 'M005',
  },
];

type PayoutStatus = 'pending' | 'approved' | 'processing' | 'completed' | 'rejected';
type TimeFilter = 'today' | 'last_hour' | 'last_3_days' | 'all';

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
    case 'processing':
      return {
        variant: 'secondary' as const,
        className: 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-100',
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

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};

const filterRequestsByTime = (requests: typeof mockPayoutRequests, filter: TimeFilter) => {
  const now = new Date();
  const nowMs = now.getTime();

  switch (filter) {
    case 'today':
      return requests.filter((request) => {
        const requestDate = new Date(request.requestDate);
        return requestDate.toDateString() === now.toDateString();
      });
    case 'last_hour':
      return requests.filter((request) => {
        const requestDate = new Date(request.requestDate);
        return nowMs - requestDate.getTime() <= 60 * 60 * 1000;
      });
    case 'last_3_days':
      return requests.filter((request) => {
        const requestDate = new Date(request.requestDate);
        return nowMs - requestDate.getTime() <= 3 * 24 * 60 * 60 * 1000;
      });
    case 'all':
    default:
      return requests;
  }
};

export function AdminPayoutRequestsPage() {
  const nav = useAppNavigation();
  const [timeFilter, setTimeFilter] = useState<TimeFilter>('all');
  const [statusFilter, setStatusFilter] = useState<PayoutStatus | 'all'>('all');
  
  // Dialog states
  const [isApproveDialogOpen, setIsApproveDialogOpen] = useState(false);
  const [isRejectDialogOpen, setIsRejectDialogOpen] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState<typeof mockPayoutRequests[0] | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  // Filter requests based on selected filters
  const filteredRequests = useMemo(() => {
    let filtered = filterRequestsByTime(mockPayoutRequests, timeFilter);
    
    if (statusFilter !== 'all') {
      filtered = filtered.filter((request) => request.status === statusFilter);
    }

    return filtered;
  }, [timeFilter, statusFilter]);

  // Calculate statistics
  const stats = useMemo(() => {
    const pendingRequests = filteredRequests.filter((req) => req.status === 'pending');
    const totalAmount = filteredRequests.reduce((sum, req) => sum + req.amount, 0);
    const pendingAmount = pendingRequests.reduce((sum, req) => sum + req.amount, 0);

    return {
      totalRequests: filteredRequests.length,
      pendingRequests: pendingRequests.length,
      totalAmount,
      pendingAmount,
    };
  }, [filteredRequests]);

  const handleApprove = async (request: typeof mockPayoutRequests[0]) => {
    setSelectedRequest(request);
    setIsApproveDialogOpen(true);
  };

  const handleReject = async (request: typeof mockPayoutRequests[0]) => {
    setSelectedRequest(request);
    setIsRejectDialogOpen(true);
  };

  const handleView = (requestId: string) => {
    nav.goToAdminPayoutRequestDetails(requestId);
  };

  const confirmApprove = async () => {
    if (!selectedRequest) return;
    
    setIsLoading(true);
    try {
      // TODO: Implement actual approve API call
      console.log('Approve request:', selectedRequest.id);
      await new Promise(resolve => setTimeout(resolve, 2000)); // Simulate API call
      setIsApproveDialogOpen(false);
      setSelectedRequest(null);
      // Update local state or refresh data
    } catch (error) {
      console.error('Error approving request:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const confirmReject = async () => {
    if (!selectedRequest) return;
    
    setIsLoading(true);
    try {
      // TODO: Implement actual reject API call
      console.log('Reject request:', selectedRequest.id);
      await new Promise(resolve => setTimeout(resolve, 2000)); // Simulate API call
      setIsRejectDialogOpen(false);
      setSelectedRequest(null);
      // Update local state or refresh data
    } catch (error) {
      console.error('Error rejecting request:', error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="mx-auto p-6 space-y-8">
      {/* Header Section */}
      <div className="space-y-2">
        <h1 className="text-4xl font-bold tracking-light">Payout Requests Management</h1>
        <p className="text-muted-foreground text-lg">
          Review and manage mentor payout requests
        </p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Total Requests</CardDescription>
            <CardTitle className="text-2xl">{stats.totalRequests}</CardTitle>
          </CardHeader>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Pending Reviews</CardDescription>
            <CardTitle className="text-2xl text-yellow-600">{stats.pendingRequests}</CardTitle>
          </CardHeader>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Total Amount</CardDescription>
            <CardTitle className="text-2xl">${stats.totalAmount.toFixed(2)}</CardTitle>
          </CardHeader>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Pending Amount</CardDescription>
            <CardTitle className="text-2xl text-yellow-600">${stats.pendingAmount.toFixed(2)}</CardTitle>
          </CardHeader>
        </Card>
      </div>

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
              <Select value={timeFilter} onValueChange={(value: TimeFilter) => setTimeFilter(value)}>
                <SelectTrigger className="w-[180px]">
                  <SelectValue placeholder="Select time period" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Time</SelectItem>
                  <SelectItem value="today">Today</SelectItem>
                  <SelectItem value="last_hour">Last Hour</SelectItem>
                  <SelectItem value="last_3_days">Last 3 Days</SelectItem>
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
                  <SelectItem value="processing">Processing</SelectItem>
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
          <CardDescription>
            {filteredRequests.length} request(s) found
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableCaption className="text-left">
              Payout requests from mentors awaiting review.
            </TableCaption>
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
              {filteredRequests.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={7} className="text-center py-8 text-muted-foreground">
                    No payout requests found for the selected filters.
                  </TableCell>
                </TableRow>
              ) : (
                filteredRequests.map((request) => {
                  const statusProps = getStatusBadgeProps(request.status);
                  return (
                    <TableRow key={request.id}>
                      <TableCell className="font-medium font-mono text-sm">
                        {request.id}
                      </TableCell>
                      <TableCell>
                        <div className="space-y-1">
                          <div className="font-medium">{request.mentorName}</div>
                          <div className="text-sm text-muted-foreground">
                            {request.mentorEmail}
                          </div>
                        </div>
                      </TableCell>
                      <TableCell className="font-semibold">
                        ${request.amount.toFixed(2)}
                      </TableCell>
                      <TableCell>{request.method}</TableCell>
                      <TableCell>
                        <Badge {...statusProps}>
                          {request.status.charAt(0).toUpperCase() + request.status.slice(1)}
                        </Badge>
                      </TableCell>
                      <TableCell className="text-sm">
                        {formatDate(request.requestDate)}
                      </TableCell>
                      <TableCell className="text-right">
                        <div className="flex items-center justify-end gap-2">
                          <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => handleView(request.id)}
                            className="h-8 w-8 p-0"
                          >
                            <Eye className="h-4 w-4" />
                          </Button>
                          {request.status === 'pending' && (
                            <>
                              <Button
                                variant="ghost"
                                size="sm"
                                onClick={() => handleApprove(request)}
                                className="h-8 w-8 p-0 text-green-600 hover:text-green-700 hover:bg-green-50"
                              >
                                <Check className="h-4 w-4" />
                              </Button>
                              <Button
                                variant="ghost"
                                size="sm"
                                onClick={() => handleReject(request)}
                                className="h-8 w-8 p-0 text-red-600 hover:text-red-700 hover:bg-red-50"
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
            onClose={() => {
              setIsApproveDialogOpen(false);
              setSelectedRequest(null);
            }}
            onConfirm={confirmApprove}
            isLoading={isLoading}
            requestId={selectedRequest.id}
            mentorName={selectedRequest.mentorName}
            amount={selectedRequest.amount}
          />

          <RejectPayoutDialog
            isOpen={isRejectDialogOpen}
            onClose={() => {
              setIsRejectDialogOpen(false);
              setSelectedRequest(null);
            }}
            onConfirm={confirmReject}
            isLoading={isLoading}
            requestId={selectedRequest.id}
            mentorName={selectedRequest.mentorName}
            amount={selectedRequest.amount}
          />
        </>
      )}
    </div>
  );
}