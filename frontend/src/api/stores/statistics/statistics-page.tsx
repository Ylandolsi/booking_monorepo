import { useState, useMemo } from 'react';
import { useGetStats } from '@/api/stores';
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from '@/components/ui';
import {
  LineChart,
  Line,
  BarChart,
  Bar,
  AreaChart,
  Area,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Legend,
  PieChart,
  Pie,
  Cell,
} from 'recharts';
import { Users, ShoppingCart, DollarSign, TrendingUp, Eye, Package, Calendar, Loader2 } from 'lucide-react';

// Types
type TimeFilter = 'day' | 'week' | 'month' | 'year' | 'all';

// Helper function to calculate date range based on filter
const getDateRange = (filter: TimeFilter): { startsAt: string; endsAt: string } => {
  const now = new Date();
  const endsAt = now.toISOString();
  let startsAt: Date;

  switch (filter) {
    case 'day':
      startsAt = new Date(now.getTime() - 24 * 60 * 60 * 1000);
      break;
    case 'week':
      startsAt = new Date(now.getTime() - 7 * 24 * 60 * 60 * 1000);
      break;
    case 'month':
      startsAt = new Date(now.getTime() - 30 * 24 * 60 * 60 * 1000);
      break;
    case 'year':
      startsAt = new Date(now.getTime() - 365 * 24 * 60 * 60 * 1000);
      break;
    case 'all':
      startsAt = new Date(now.getTime() - 365 * 24 * 60 * 60 * 1000); // Default to 1 year for 'all'
      break;
  }

  return {
    startsAt: startsAt.toISOString(),
    endsAt,
  };
};

interface Product {
  id: string;
  name: string;
  sales: number;
  revenue: number;
  trend: number;
  [key: string]: string | number; // Add index signature
}

const COLORS = ['#10b981', '#3b82f6', '#f59e0b', '#ef4444', '#8b5cf6'];

export function AnalyticsPage() {
  const [timeFilter, setTimeFilter] = useState<TimeFilter>('month');

  // Calculate date range based on time filter
  const dateRange = useMemo(() => getDateRange(timeFilter), [timeFilter]);

  // Fetch real data from API
  const {
    data: statsData,
    isLoading,
    error,
  } = useGetStats({
    type: 'all',
    startsAt: dateRange.startsAt,
    endsAt: dateRange.endsAt,
  });

  // Transform API data to match chart format
  const chartData = useMemo(() => {
    if (!statsData?.chartData) return [];
    return statsData.chartData.map((item) => ({
      name: item.date,
      revenue: item.revenue,
      sales: item.sales,
      customers: item.customers,
      visitors: item.visitors,
    }));
  }, [statsData]);

  const productData = useMemo(() => {
    if (!statsData?.productData) return [];
    return statsData.productData.map((item) => ({
      id: item.productSlug,
      name: item.name,
      sales: item.sales,
      revenue: item.revenue,
    }));
  }, [statsData]);

  const totals = useMemo(() => {
    if (!statsData?.totals) {
      return {
        revenue: 0,
        sales: 0,
        customers: 0,
        visitors: 0,
        averageRevenue: 0,
        averageSales: 0,
        conversionRate: '0.0',
      };
    }
    return statsData.totals;
  }, [statsData]);

  const bestSellingProduct = useMemo(() => {
    if (productData.length === 0) return null;
    return productData.reduce((prev, current) => (prev.sales > current.sales ? prev : current));
  }, [productData]);

  // Loading state
  if (isLoading) {
    return (
      <div className="bg-background flex min-h-screen items-center justify-center p-4 md:p-6 lg:p-8">
        <div className="flex flex-col items-center gap-4">
          <Loader2 className="text-primary h-8 w-8 animate-spin" />
          <p className="text-muted-foreground">Loading analytics data...</p>
        </div>
      </div>
    );
  }

  // Error state
  if (error) {
    return (
      <div className="bg-background flex min-h-screen items-center justify-center p-4 md:p-6 lg:p-8">
        <Card className="w-full max-w-md">
          <CardHeader>
            <CardTitle className="text-destructive">Error Loading Analytics</CardTitle>
            <CardDescription>Failed to load analytics data. Please try again later.</CardDescription>
          </CardHeader>
          <CardContent>
            <p className="text-muted-foreground text-sm">{error.message}</p>
          </CardContent>
        </Card>
      </div>
    );
  }

  // Empty state
  if (!statsData || chartData.length === 0) {
    return (
      <div className="bg-background flex min-h-screen items-center justify-center p-4 md:p-6 lg:p-8">
        <Card className="w-full max-w-md">
          <CardHeader>
            <CardTitle>No Data Available</CardTitle>
            <CardDescription>There's no analytics data for the selected time period.</CardDescription>
          </CardHeader>
          <CardContent>
            <p className="text-muted-foreground text-sm">Start getting customers and sales to see your analytics dashboard populate with insights.</p>
          </CardContent>
        </Card>
      </div>
    );
  }

  return (
    <div className="bg-background min-h-screen p-4 md:p-6 lg:p-8">
      <div className="mx-auto max-w-7xl space-y-6">
        {/* Header */}
        <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
          <div>
            <h1 className="text-3xl font-bold tracking-tight">Analytics Dashboard</h1>
            <p className="text-muted-foreground mt-1">Track your business performance and metrics</p>
          </div>

          <div className="flex items-center gap-3">
            <Select value={timeFilter} onValueChange={(value: TimeFilter) => setTimeFilter(value)}>
              <SelectTrigger className="w-[180px]">
                <Calendar className="mr-2 h-4 w-4" />
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="day">Last 24 Hours</SelectItem>
                <SelectItem value="week">Last Week</SelectItem>
                <SelectItem value="month">Last Month</SelectItem>
                <SelectItem value="year">Last Year</SelectItem>
                <SelectItem value="all">All Time</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>

        {/* Stats Cards */}
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          {/* Total Customers */}
          <Card className="transition-shadow hover:shadow-lg">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Total Customers</CardTitle>
              <Users className="text-muted-foreground h-4 w-4" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{totals.customers.toLocaleString()}</div>
              <p className="text-muted-foreground mt-1 flex items-center text-xs">
                <TrendingUp className="mr-1 h-3 w-3 text-green-500" />
                <span className="text-green-500">+12.5%</span>
                <span className="ml-1">from last period</span>
              </p>
            </CardContent>
          </Card>

          {/* Total Visitors */}
          <Card className="transition-shadow hover:shadow-lg">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Shop Visitors</CardTitle>
              <Eye className="text-muted-foreground h-4 w-4" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{totals.visitors.toLocaleString()}</div>
              <p className="text-muted-foreground mt-1 flex items-center text-xs">
                <TrendingUp className="mr-1 h-3 w-3 text-green-500" />
                <span className="text-green-500">+8.2%</span>
                <span className="ml-1">from last period</span>
              </p>
              <div className="bg-muted mt-2 rounded-full">
                <div className="bg-primary h-1 rounded-full" style={{ width: `${totals.conversionRate}%` }} />
              </div>
              <p className="text-muted-foreground mt-1 text-xs">Conversion Rate: {totals.conversionRate}%</p>
            </CardContent>
          </Card>

          {/* Total Revenue */}
          <Card className="transition-shadow hover:shadow-lg">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Total Revenue</CardTitle>
              <DollarSign className="text-muted-foreground h-4 w-4" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">${totals.revenue.toLocaleString()}</div>
              <p className="text-muted-foreground mt-1 flex items-center text-xs">
                <TrendingUp className="mr-1 h-3 w-3 text-green-500" />
                <span className="text-green-500">+15.3%</span>
                <span className="ml-1">from last period</span>
              </p>
            </CardContent>
          </Card>

          {/* Total Sales */}
          <Card className="transition-shadow hover:shadow-lg">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Total Sales</CardTitle>
              <ShoppingCart className="text-muted-foreground h-4 w-4" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{totals.sales.toLocaleString()}</div>
              <p className="text-muted-foreground mt-1 flex items-center text-xs">
                <TrendingUp className="mr-1 h-3 w-3 text-green-500" />
                <span className="text-green-500">+9.7%</span>
                <span className="ml-1">from last period</span>
              </p>
            </CardContent>
          </Card>
        </div>

        {/* Best Selling Product Highlight */}
        {bestSellingProduct && (
          <Card className="border-primary from-primary/5 to-primary/10 bg-gradient-to-r">
            <CardHeader>
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-3">
                  <div className="bg-primary rounded-lg p-3">
                    <Package className="h-6 w-6 text-white" />
                  </div>
                  <div>
                    <CardTitle className="text-lg">Best Selling Product</CardTitle>
                    <CardDescription className="mt-1">{bestSellingProduct.name}</CardDescription>
                  </div>
                </div>
              </div>
            </CardHeader>
            <CardContent>
              <div className="grid gap-4 md:grid-cols-3">
                <div>
                  <p className="text-muted-foreground text-sm">Total Sales</p>
                  <p className="text-2xl font-bold">{bestSellingProduct.sales.toLocaleString()}</p>
                </div>
                <div>
                  <p className="text-muted-foreground text-sm">Revenue Generated</p>
                  <p className="text-2xl font-bold">${bestSellingProduct.revenue.toLocaleString()}</p>
                </div>
                <div>
                  <p className="text-muted-foreground text-sm">Average Price</p>
                  <p className="text-2xl font-bold">${(bestSellingProduct.revenue / bestSellingProduct.sales).toFixed(2)}</p>
                </div>
              </div>
            </CardContent>
          </Card>
        )}

        {/* Charts Section */}
        <Tabs defaultValue="revenue" className="space-y-4">
          <TabsList className="grid w-full grid-cols-2 lg:w-[400px]">
            <TabsTrigger value="revenue">Revenue</TabsTrigger>
            <TabsTrigger value="sales">Sales</TabsTrigger>
          </TabsList>

          {/* Revenue Tab */}
          <TabsContent value="revenue" className="space-y-4">
            <div className="grid gap-4 lg:grid-cols-2">
              {/* Revenue Over Time */}
              <Card>
                <CardHeader>
                  <CardTitle>Revenue Over Time</CardTitle>
                  <CardDescription>Track revenue trends across the selected period</CardDescription>
                </CardHeader>
                <CardContent>
                  <ResponsiveContainer width="100%" height={300}>
                    <AreaChart data={chartData}>
                      <defs>
                        <linearGradient id="colorRevenue" x1="0" y1="0" x2="0" y2="1">
                          <stop offset="5%" stopColor="#10b981" stopOpacity={0.8} />
                          <stop offset="95%" stopColor="#10b981" stopOpacity={0.1} />
                        </linearGradient>
                      </defs>
                      <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                      <XAxis dataKey="name" className="text-xs" />
                      <YAxis className="text-xs" />
                      <Tooltip
                        contentStyle={{
                          backgroundColor: 'hsl(var(--background))',
                          border: '1px solid hsl(var(--border))',
                          borderRadius: '8px',
                        }}
                      />
                      <Area type="monotone" dataKey="revenue" stroke="#10b981" fillOpacity={1} fill="url(#colorRevenue)" />
                    </AreaChart>
                  </ResponsiveContainer>
                </CardContent>
              </Card>

              {/* Revenue by Product */}
              <Card>
                <CardHeader>
                  <CardTitle>Revenue by Product</CardTitle>
                  <CardDescription>Distribution of revenue across products</CardDescription>
                </CardHeader>
                <CardContent className="flex items-center justify-center">
                  <ResponsiveContainer width="100%" height={300}>
                    <PieChart>
                      <Pie data={productData} cx="50%" cy="50%" labelLine={false} outerRadius={80} fill="#8884d8" dataKey="revenue" label>
                        {productData.map((entry, index) => (
                          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                        ))}
                      </Pie>
                      <Tooltip
                        contentStyle={{
                          backgroundColor: 'hsl(var(--background))',
                          border: '1px solid hsl(var(--border))',
                          borderRadius: '8px',
                        }}
                      />
                      <Legend />
                    </PieChart>
                  </ResponsiveContainer>
                </CardContent>
              </Card>
            </div>

            {/* Product Revenue Table */}
            <Card>
              <CardHeader>
                <CardTitle>Product Revenue Breakdown</CardTitle>
                <CardDescription>Detailed revenue metrics per product</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-3">
                  {productData.map((product, index) => (
                    <div key={product.id} className="border-border flex items-center justify-between rounded-lg border p-4">
                      <div className="flex items-center gap-4">
                        <div className="flex h-10 w-10 items-center justify-center rounded-lg" style={{ backgroundColor: COLORS[index] }}>
                          <span className="font-bold text-white">{index + 1}</span>
                        </div>
                        <div>
                          <p className="font-semibold">{product.name}</p>
                          <p className="text-muted-foreground text-sm">{product.sales} sales</p>
                        </div>
                      </div>
                      <div className="text-right">
                        <p className="text-lg font-bold">${product.revenue.toLocaleString()}</p>
                      </div>
                    </div>
                  ))}
                </div>
              </CardContent>
            </Card>
          </TabsContent>

          {/* Sales Tab */}
          <TabsContent value="sales" className="space-y-4">
            <div className="grid gap-4 lg:grid-cols-2">
              {/* Sales Over Time */}
              <Card>
                <CardHeader>
                  <CardTitle>Sales Over Time</CardTitle>
                  <CardDescription>Track sales volume trends</CardDescription>
                </CardHeader>
                <CardContent>
                  <ResponsiveContainer width="100%" height={300}>
                    <LineChart data={chartData}>
                      <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                      <XAxis dataKey="name" className="text-xs" />
                      <YAxis className="text-xs" />
                      <Tooltip
                        contentStyle={{
                          backgroundColor: 'hsl(var(--background))',
                          border: '1px solid hsl(var(--border))',
                          borderRadius: '8px',
                        }}
                      />
                      <Legend />
                      <Line type="monotone" dataKey="sales" stroke="#3b82f6" strokeWidth={2} dot={{ r: 4 }} activeDot={{ r: 6 }} />
                    </LineChart>
                  </ResponsiveContainer>
                </CardContent>
              </Card>

              {/* Sales by Product */}
              <Card>
                <CardHeader>
                  <CardTitle>Sales by Product</CardTitle>
                  <CardDescription>Compare sales volume across products</CardDescription>
                </CardHeader>
                <CardContent>
                  <ResponsiveContainer width="100%" height={300}>
                    <BarChart data={productData}>
                      <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                      <XAxis dataKey="name" className="text-xs" angle={-45} textAnchor="end" height={100} />
                      <YAxis className="text-xs" />
                      <Tooltip
                        contentStyle={{
                          backgroundColor: 'hsl(var(--background))',
                          border: '1px solid hsl(var(--border))',
                          borderRadius: '8px',
                        }}
                      />
                      <Bar dataKey="sales" fill="#3b82f6" radius={[8, 8, 0, 0]} />
                    </BarChart>
                  </ResponsiveContainer>
                </CardContent>
              </Card>
            </div>

            {/* Product Sales Table */}
            <Card>
              <CardHeader>
                <CardTitle>Product Sales Performance</CardTitle>
                <CardDescription>Detailed sales metrics per product</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-3">
                  {productData.map((product, index) => (
                    <div key={product.id} className="border-border flex items-center justify-between rounded-lg border p-4">
                      <div className="flex items-center gap-4">
                        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-blue-500">
                          <span className="font-bold text-white">{index + 1}</span>
                        </div>
                        <div>
                          <p className="font-semibold">{product.name}</p>
                          <p className="text-muted-foreground text-sm">${(product.revenue / product.sales).toFixed(2)} avg. price</p>
                        </div>
                      </div>
                      <div className="text-right">
                        <p className="text-lg font-bold">{product.sales} units</p>
                      </div>
                    </div>
                  ))}
                </div>
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>

        {/* Customer & Visitor Analytics */}
        <div className="grid gap-4 lg:grid-cols-2">
          {/* Customers Over Time */}
          <Card>
            <CardHeader>
              <CardTitle>Customer Acquisition</CardTitle>
              <CardDescription>New customers over time</CardDescription>
            </CardHeader>
            <CardContent>
              <ResponsiveContainer width="100%" height={250}>
                <AreaChart data={chartData}>
                  <defs>
                    <linearGradient id="colorCustomers" x1="0" y1="0" x2="0" y2="1">
                      <stop offset="5%" stopColor="#8b5cf6" stopOpacity={0.8} />
                      <stop offset="95%" stopColor="#8b5cf6" stopOpacity={0.1} />
                    </linearGradient>
                  </defs>
                  <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                  <XAxis dataKey="name" className="text-xs" />
                  <YAxis className="text-xs" />
                  <Tooltip
                    contentStyle={{
                      backgroundColor: 'hsl(var(--background))',
                      border: '1px solid hsl(var(--border))',
                      borderRadius: '8px',
                    }}
                  />
                  <Area type="monotone" dataKey="customers" stroke="#8b5cf6" fillOpacity={1} fill="url(#colorCustomers)" />
                </AreaChart>
              </ResponsiveContainer>
            </CardContent>
          </Card>

          {/* Visitors vs Customers */}
          <Card>
            <CardHeader>
              <CardTitle>Visitors vs Customers</CardTitle>
              <CardDescription>Conversion comparison</CardDescription>
            </CardHeader>
            <CardContent>
              <ResponsiveContainer width="100%" height={250}>
                <BarChart data={chartData}>
                  <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                  <XAxis dataKey="name" className="text-xs" />
                  <YAxis className="text-xs" />
                  <Tooltip
                    contentStyle={{
                      backgroundColor: 'hsl(var(--background))',
                      border: '1px solid hsl(var(--border))',
                      borderRadius: '8px',
                    }}
                  />
                  <Legend />
                  <Bar dataKey="visitors" fill="#f59e0b" radius={[8, 8, 0, 0]} />
                  <Bar dataKey="customers" fill="#10b981" radius={[8, 8, 0, 0]} />
                </BarChart>
              </ResponsiveContainer>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
