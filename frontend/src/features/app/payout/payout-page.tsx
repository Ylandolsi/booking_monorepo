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
} from '@/components';
import { DollarSign, Calendar, TrendingUp } from 'lucide-react';

export function PayoutPage() {
  return (
    <div className="mx-auto p-6 space-y-8">
      {/* Header Section */}
      <div className="space-y-2">
        <h1 className="text-4xl font-bold tracking-light">Payouts</h1>
        <p className="text-muted-foreground text-lg">
          Manage your payout methods and view your transaction history
        </p>
      </div>

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
              <div className="text-4xl font-bold text-primary">$2,222.00</div>
              <div className="text-sm text-muted-foreground flex items-center gap-1">
                <TrendingUp className="h-4 w-4" />
                Ready to withdraw
              </div>
            </div>
            <Button size="lg" className="bg-primary hover:bg-primary/80">
              Request Payout
            </Button>
          </div>
        </CardContent>
      </Card>

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
                  <TableHead>Method</TableHead>
                  <TableHead>Status</TableHead>
                  <TableHead className="text-right">Transaction ID</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                <TableRow>
                  <TableCell className="font-medium">May 15, 2024</TableCell>
                  <TableCell className="font-semibold">$1,234.56</TableCell>
                  <TableCell>Bank Transfer</TableCell>
                  <TableCell>
                    <Badge
                      variant="default"
                      className="bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-100"
                    >
                      Completed
                    </Badge>
                  </TableCell>
                  <TableCell className="text-right text-muted-foreground font-mono text-sm">
                    TXN-001234
                  </TableCell>
                </TableRow>
                <TableRow>
                  <TableCell className="font-medium">April 28, 2024</TableCell>
                  <TableCell className="font-semibold">$892.30</TableCell>
                  <TableCell>Bank Transfer</TableCell>
                  <TableCell>
                    <Badge
                      variant="default"
                      className="bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-100"
                    >
                      Completed
                    </Badge>
                  </TableCell>
                  <TableCell className="text-right text-muted-foreground font-mono text-sm">
                    TXN-001189
                  </TableCell>
                </TableRow>
                <TableRow>
                  <TableCell className="font-medium">April 10, 2024</TableCell>
                  <TableCell className="font-semibold">$567.89</TableCell>
                  <TableCell>PayPal</TableCell>
                  <TableCell>
                    <Badge
                      variant="secondary"
                      className="bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-100"
                    >
                      Processing
                    </Badge>
                  </TableCell>
                  <TableCell className="text-right text-muted-foreground font-mono text-sm">
                    TXN-001156
                  </TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </CardContent>
        </Card>
      </div>

      {/* Quick Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <Card>
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
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Pending</CardDescription>
            <CardTitle className="text-2xl">$567.89</CardTitle>
          </CardHeader>
        </Card>
      </div>
    </div>
  );
}
