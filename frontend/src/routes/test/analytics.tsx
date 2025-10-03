import { createFileRoute } from '@tanstack/react-router';
import { useState, useMemo } from 'react';
import { ROUTE_PATHS } from '@/config/routes';
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
  Badge,
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
import { Users, ShoppingCart, DollarSign, TrendingUp, TrendingDown, Eye, Package, Calendar, ArrowUpRight, ArrowDownRight } from 'lucide-react';

export const Route = createFileRoute(ROUTE_PATHS.TEST.ANALYTICS)({
  component: RouteComponent,
});

function RouteComponent() {
  return <AnalyticsPage />;
}

// Types
type TimeFilter = 'day' | 'week' | 'month' | 'year' | 'all';

interface Product {
  id: string;
  name: string;
  sales: number;
  revenue: number;
  trend: number;
  [key: string]: string | number; // Add index signature
}

// Mock Data Generator
const generateMockData = (filter: TimeFilter) => {
  const dataPoints = {
    day: 24,
    week: 7,
    month: 30,
    year: 12,
    all: 12,
  };

  const points = dataPoints[filter];
  const data = [];

  for (let i = 0; i < points; i++) {
    const baseRevenue = Math.random() * 5000 + 2000;
    const baseSales = Math.floor(Math.random() * 50 + 20);
    const baseCustomers = Math.floor(Math.random() * 100 + 50);
    const baseVisitors = Math.floor(Math.random() * 300 + 100);

    data.push({
      name:
        filter === 'day'
          ? `${i}:00`
          : filter === 'week'
            ? ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'][i]
            : filter === 'month'
              ? `Day ${i + 1}`
              : filter === 'year'
                ? ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][i]
                : `M${i + 1}`,
      revenue: Math.round(baseRevenue),
      sales: baseSales,
      customers: baseCustomers,
      visitors: baseVisitors,
    });
  }

  return data;
};

const generateProductData = (): Product[] => [
  { id: '1', name: 'Premium Booking Session', sales: 234, revenue: 23400, trend: 12.5 },
  { id: '2', name: 'Standard Consultation', sales: 189, revenue: 18900, trend: 8.3 },
  { id: '3', name: 'Quick Call', sales: 156, revenue: 7800, trend: -3.2 },
  { id: '4', name: 'Workshop Package', sales: 98, revenue: 19600, trend: 15.7 },
  { id: '5', name: 'Digital Course', sales: 67, revenue: 6700, trend: -5.1 },
];

const COLORS = ['#10b981', '#3b82f6', '#f59e0b', '#ef4444', '#8b5cf6'];

function AnalyticsPage() {
  const [timeFilter, setTimeFilter] = useState<TimeFilter>('month');

  const chartData = useMemo(() => generateMockData(timeFilter), [timeFilter]);
  const productData = useMemo(() => generateProductData(), []);
  console.log('chartData:', chartData);
  console.log('productData:', productData);
  // Calculate totals
  const totals = useMemo(() => {
    const total = chartData.reduce(
      (acc, curr) => ({
        revenue: acc.revenue + curr.revenue,
        sales: acc.sales + curr.sales,
        customers: acc.customers + curr.customers,
        visitors: acc.visitors + curr.visitors,
      }),
      { revenue: 0, sales: 0, customers: 0, visitors: 0 },
    );

    return {
      ...total,
      averageRevenue: Math.round(total.revenue / chartData.length),
      averageSales: Math.round(total.sales / chartData.length),
      conversionRate: ((total.customers / total.visitors) * 100).toFixed(1),
    };
  }, [chartData]);

  console.log('totals:', totals);

  const bestSellingProduct = productData.reduce((prev, current) => (prev.sales > current.sales ? prev : current));

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
              <Badge variant="default" className="px-4 py-2 text-base">
                <ArrowUpRight className="mr-1 h-4 w-4" />
                {bestSellingProduct.trend > 0 ? '+' : ''}
                {bestSellingProduct.trend}%
              </Badge>
            </div>
          </CardHeader>
          <CardContent>
            <div className="grid gap-4 md:grid-cols-3">
              <div>
                <p className="text-muted-foreground text-sm">Total Sales</p>
                <p className="text-2xl font-bold">{bestSellingProduct.sales}</p>
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
                        <div className="flex items-center justify-end gap-1">
                          {product.trend > 0 ? <TrendingUp className="h-3 w-3 text-green-500" /> : <TrendingDown className="h-3 w-3 text-red-500" />}
                          <span className={`text-xs ${product.trend > 0 ? 'text-green-500' : 'text-red-500'}`}>
                            {product.trend > 0 ? '+' : ''}
                            {product.trend}%
                          </span>
                        </div>
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
                        <div className="flex items-center justify-end gap-1">
                          {product.trend > 0 ? <TrendingUp className="h-3 w-3 text-green-500" /> : <TrendingDown className="h-3 w-3 text-red-500" />}
                          <span className={`text-xs ${product.trend > 0 ? 'text-green-500' : 'text-red-500'}`}>
                            {product.trend > 0 ? '+' : ''}
                            {product.trend}%
                          </span>
                        </div>
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
