import { useGetStats } from '@/api/stores';
import type { TimeFilter } from '@/pages/store/private/statistics';
import { useMemo } from 'react';

export const useStatsTransformed = ({ timeFilter }: { timeFilter: TimeFilter }) => {
  // memorize to avoid unnecessary recalculations when seconds/minutes change
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
  const chartData = !statsData?.chartData
    ? []
    : statsData.chartData.map((item) => ({
        name: item.date,
        revenue: item.revenue,
        sales: item.sales,
        customers: item.customers,
        visitors: item.visitors,
      }));

  const productData = !statsData?.productData
    ? []
    : statsData.productData.map((item) => ({
        id: item.productSlug,
        name: item.name,
        sales: item.sales,
        revenue: item.revenue,
      }));

  const totals = !statsData?.totals
    ? {
        revenue: 0,
        sales: 0,
        customers: 0,
        visitors: 0,
        averageRevenue: 0,
        averageSales: 0,
        conversionRate: '0.0',
      }
    : statsData.totals;

  const bestSellingProduct = productData.length === 0 ? null : productData.reduce((prev, current) => (prev.sales > current.sales ? prev : current));

  return {
    chartData,
    statsData,
    productData,
    totals,
    bestSellingProduct,
    isLoading,
    error,
  };
};

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
