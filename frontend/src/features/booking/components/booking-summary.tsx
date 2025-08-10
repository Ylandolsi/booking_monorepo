import { Calendar, Clock, DollarSign, User } from 'lucide-react';
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Avatar,
  AvatarImage,
  AvatarFallback,
  Badge,
} from '@/components/ui';
import { cn } from '@/utils';
import type { BookingSummary } from '../types/booking-types';

interface BookingSummaryProps {
  booking: BookingSummary;
  className?: string;
}

export function BookingSummaryCard({
  booking,
  className,
}: BookingSummaryProps) {
  const formatDate = (dateString: string): string => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

  const getInitials = (name: string): string => {
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase();
  };

  return (
    <Card className={cn('w-full', className)}>
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Calendar className="w-5 h-5" />
          Booking Summary
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-6">
        {/* Mentor Info */}
        <div className="flex items-start gap-4 p-4 bg-gray-50 rounded-lg">
          <Avatar className="w-12 h-12">
            <AvatarImage
              src={booking.mentor.avatar}
              alt={booking.mentor.name}
            />
            <AvatarFallback>{getInitials(booking.mentor.name)}</AvatarFallback>
          </Avatar>
          <div className="flex-1">
            <h3 className="font-semibold text-lg">{booking.mentor.name}</h3>
            {booking.mentor.title && (
              <p className="text-sm text-muted-foreground mb-2">
                {booking.mentor.title}
              </p>
            )}
            <div className="flex flex-wrap gap-1">
              {booking.mentor.expertise.slice(0, 3).map((skill, index) => (
                <Badge key={index} variant="secondary" className="text-xs">
                  {skill}
                </Badge>
              ))}
              {booking.mentor.expertise.length > 3 && (
                <Badge variant="secondary" className="text-xs">
                  +{booking.mentor.expertise.length - 3} more
                </Badge>
              )}
            </div>
          </div>
          <div className="text-right">
            <div className="text-sm text-muted-foreground">Hourly Rate</div>
            <div className="font-semibold">${booking.mentor.hourlyRate}</div>
          </div>
        </div>

        {/* Session Details */}
        <div className="space-y-4">
          <h4 className="font-medium text-base">Session Details</h4>

          <div className="grid gap-3">
            <div className="flex items-center gap-3">
              <Calendar className="w-4 h-4 text-blue-600" />
              <div>
                <div className="font-medium">Date</div>
                <div className="text-sm text-muted-foreground">
                  {formatDate(booking.session.date)}
                </div>
              </div>
            </div>

            <div className="flex items-center gap-3">
              <Clock className="w-4 h-4 text-green-600" />
              <div>
                <div className="font-medium">Time</div>
                <div className="text-sm text-muted-foreground">
                  {booking.session.time} ({booking.session.duration} minutes)
                </div>
              </div>
            </div>

            <div className="flex items-center gap-3">
              <DollarSign className="w-4 h-4 text-purple-600" />
              <div>
                <div className="font-medium">Session Price</div>
                <div className="text-sm text-muted-foreground">
                  {booking.session.currency}
                  {booking.session.price}
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Pricing Breakdown */}
        <div className="border-t pt-4">
          <div className="space-y-2">
            <div className="flex justify-between text-sm">
              <span>Session ({booking.session.duration} min)</span>
              <span>
                {booking.session.currency}
                {booking.session.price}
              </span>
            </div>
            <div className="flex justify-between text-sm">
              <span>Platform fee</span>
              <span>{booking.session.currency}0</span>
            </div>
            <div className="border-t pt-2">
              <div className="flex justify-between font-semibold text-lg">
                <span>Total</span>
                <span>
                  {booking.session.currency}
                  {booking.total}
                </span>
              </div>
            </div>
          </div>
        </div>

        {/* Additional Info */}
        <div className="bg-blue-50 p-4 rounded-lg border border-blue-200">
          <h5 className="font-medium text-blue-900 mb-2">What to expect:</h5>
          <ul className="text-sm text-blue-800 space-y-1">
            <li>• Video call link will be sent via email</li>
            <li>• Join 5 minutes before the session starts</li>
            <li>• Prepare any questions or topics you'd like to discuss</li>
            <li>• Session recording available upon request</li>
          </ul>
        </div>
      </CardContent>
    </Card>
  );
}
