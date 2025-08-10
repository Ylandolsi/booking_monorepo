import { BookingPage } from '@/features/booking';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/test/booking-demo')({
  component: BookingDemo,
});

function BookingDemo() {
  return (
    <div className="min-h-screen bg-gray-50">
      <div className="bg-blue-600 text-white py-4 mb-6">
        <div className="container mx-auto px-4">
          <h1 className="text-xl font-semibold">🎯 Booking System Demo</h1>
          <p className="text-blue-100 text-sm">
            This is a demonstration of the mentor booking system with mock data
          </p>
        </div>
      </div>

      {/* Pass a demo mentor slug */}
      <BookingPage />

      <div className="container mx-auto px-4 py-8 max-w-4xl">
        <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
          <h3 className="font-medium text-yellow-800 mb-2">📝 Demo Notes:</h3>
          <ul className="text-sm text-yellow-700 space-y-1">
            <li>
              • This demo uses mock mentor data (mentorSlug: "demo-mentor")
            </li>
            <li>• Calendar dates are randomly generated for demonstration</li>
            <li>
              • Time slots are simulated - actual booking will require real API
              integration
            </li>
            <li>• The booking flow shows all the key features of the system</li>
            <li>• Responsive design works on mobile, tablet, and desktop</li>
          </ul>
        </div>
      </div>
    </div>
  );
}
