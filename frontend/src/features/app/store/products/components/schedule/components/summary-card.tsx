import { Card, CardContent, CardHeader, CardTitle } from '@/components';
import { Settings } from 'lucide-react';

interface SummaryCardProps {
  summary: {
    enabledDays: number;
    totalSlots: number;
  };
}

export function SummaryCard({ summary }: SummaryCardProps) {
  const minutesPerSlot = 30;
  const totalMinutes = summary.totalSlots * minutesPerSlot;

  return (
    <Card className="mb-8">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Settings className="w-5 h-5" />
          Availability Summary
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
          <div className="text-center">
            <div className="text-2xl font-bold text-blue-600">
              {summary.enabledDays}
            </div>
            <div className="text-sm text-gray-600">Active Days</div>
          </div>
          <div className="text-center">
            <div className="text-2xl font-bold text-green-600">
              {summary.totalSlots}
            </div>
            <div className="text-sm text-gray-600">Time Slots</div>
          </div>
          <div className="text-center">
            <div className="text-2xl font-bold text-purple-600">
              {totalMinutes}
            </div>
            <div className="text-sm text-gray-600">Minutes/Week</div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
