# Booking System Documentation

## Overview

The booking system allows users to schedule mentoring sessions with experienced mentors. It provides a comprehensive interface for selecting dates, time slots, viewing mentor profiles, and confirming bookings.

## Features

### 📅 Calendar-Based Date Selection

- Interactive monthly calendar view
- Visual indicators for available dates
- Month navigation (previous/next)
- Date availability highlighting

### ⏰ Time Slot Management

- 30-minute session slots (fixed duration)
- Real-time availability updates
- Time slot selection with visual feedback
- Session pricing display

### 👤 Mentor Profile Display

- Mentor information (name, title, expertise)
- Hourly rate display
- Avatar/profile picture
- Area of expertise badges

### 📋 Booking Summary

- Real-time booking details update
- Session information (date, time, duration)
- Cost breakdown and total
- Mentor profile summary

### ✅ Booking Confirmation

- Secure session booking
- Success confirmation page
- Email notification promise
- Navigation to booking management

## Components

### BookingPage

Main container component that orchestrates the entire booking flow.

**Features:**

- State management for selected date/time
- API integration for mentor and availability data
- Multi-step booking process
- Error handling and loading states

### Calendar

Interactive calendar component for date selection.

**Props:**

- `selectedDate`: Currently selected date
- `onDateSelect`: Date selection handler
- `availableDates`: Array of available dates
- `className`: Optional styling

### TimeSlots

Time slot selection component.

**Props:**

- `selectedDate`: Selected date
- `timeSlots`: Available time slots for the date
- `selectedSlot`: Currently selected slot
- `onSlotSelect`: Slot selection handler
- `isLoading`: Loading state
- `className`: Optional styling

### BookingSummaryCard

Displays booking details and confirmation.

**Props:**

- `booking`: Booking summary object
- `className`: Optional styling

## Usage

### Routing

The booking page is accessible via: `/booking/$mentorSlug`

Example: `/booking/john-doe`

### API Integration

The system integrates with several APIs:

1. **Mentor Details API** - Fetches mentor profile information
2. **Monthly Availability API** - Gets available dates for the current month
3. **Daily Availability API** - Retrieves time slots for a specific date
4. **Book Session API** - Creates a new booking

### State Management

```typescript
const [selectedDate, setSelectedDate] = useState<string | null>(null);
const [selectedSlot, setSelectedSlot] = useState<TimeSlot | null>(null);
const [bookingStep, setBookingStep] = useState<'select' | 'success'>('select');
```

## User Flow

1. **Browse the Agenda** - User views the monthly calendar with available dates
2. **Select a Date** - User clicks on an available date to see time slots
3. **Choose a Time** - User selects a preferred 30-minute time slot
4. **Review & Confirm** - User reviews booking summary and confirms

## Session Details

- **Duration**: 30 minutes (fixed, but customizable in the future)
- **Price**: $50 or mentor-specific pricing
- **Format**: Online video call
- **Platform Fee**: $0 (for now)

## Technical Notes

### Type Definitions

```typescript
interface TimeSlot {
  id: string;
  start: string; // ISO string format
  end: string; // ISO string format
  isAvailable: boolean;
  isBooked: boolean;
}

interface BookingSummary {
  mentor: {
    slug: string;
    name: string;
    avatar?: string;
    title?: string;
    expertise: string[];
    hourlyRate: number;
  };
  session: {
    date: string;
    time: string;
    duration: number;
    price: number;
    currency: string;
  };
  total: number;
}
```

### Error Handling

The system provides comprehensive error handling:

- Network errors
- API failures
- Invalid mentor slugs
- Booking conflicts
- Loading states

### Responsive Design

The booking interface is fully responsive:

- Mobile-first design
- Tablet optimization
- Desktop layout with sidebar
- Touch-friendly interactions

## Future Enhancements

1. **Customizable Session Duration** - Allow users to select different session lengths
2. **Multiple Mentor Selection** - Compare and book with multiple mentors
3. **Payment Integration** - Secure payment processing
4. **Calendar Integration** - Export to user's calendar
5. **Video Call Integration** - Direct meeting link generation
6. **Booking Management** - View, reschedule, and cancel bookings
7. **Recurring Sessions** - Schedule regular mentoring sessions

## Files Structure

```
src/features/booking/
├── pages/
│   ├── booking-page-2.tsx
│   └── index.ts
├── components/
│   ├── calendar.tsx
│   ├── time-slots.tsx
│   ├── booking-summary.tsx
│   └── index.ts
├── api/
│   ├── availability.ts
│   ├── book-session.ts
│   └── index.ts
├── types/
│   ├── availability.ts
│   ├── booking.ts
│   ├── session.ts
│   └── index.ts
└── index.ts
```

## Getting Started

1. Navigate to `/booking/{mentorSlug}` in your application
2. Select an available date from the calendar
3. Choose a preferred time slot
4. Review the booking summary
5. Click "Book Session" to confirm

The system will handle all the API calls and provide feedback throughout the process.
