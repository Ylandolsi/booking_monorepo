import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';

import React from 'react';
import { useUser } from '@/features/auth';
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Badge,
  Link,
} from '@/components/ui';
import {
  CheckCircle,
  Calendar,
  Users,
  Star,
  DollarSign,
  ArrowRight,
  Settings,
  BarChart3,
  MessageCircle,
  Award,
} from 'lucide-react';

// Mock mentor data - replace with real API
const mockMentorData = {
  joinDate: '2024-02-15',
  totalSessions: 42,
  totalEarnings: 2450.0,
  averageRating: 4.8,
  totalStudents: 28,
  upcomingSessions: 3,
  status: 'active', // active, pending, suspended
  hourlyRate: 45.0,
  bufferTime: 15,
  specialties: ['React', 'JavaScript', 'Node.js', 'Python'],
};

export function AlreadyMentorPage() {
  const { data: user } = useUser();
  const mentorData = mockMentorData; // Replace with actual API call

  const formatDate = (dateStr: string) => {
    return new Date(dateStr).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'active':
        return (
          <Badge className="bg-green-100 text-green-800">Active Mentor</Badge>
        );
      case 'pending':
        return (
          <Badge className="bg-yellow-100 text-yellow-800">
            Pending Approval
          </Badge>
        );
      case 'suspended':
        return <Badge className="bg-red-100 text-red-800">Suspended</Badge>;
      default:
        return <Badge variant="secondary">Unknown Status</Badge>;
    }
  };

  return (
    <div className="container mx-auto py-10 px-4 max-w-4xl">
      {/* Header Section */}
      <div className="text-center space-y-6 mb-8">
        <div className="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center mx-auto">
          <CheckCircle className="w-10 h-10 text-green-600" />
        </div>

        <div className="space-y-2">
          <h1 className="text-3xl font-bold text-gray-900">
            You're Already a Mentor!
          </h1>
          <p className="text-lg text-gray-600 max-w-2xl mx-auto">
            Welcome back, {user?.firstName || 'there'}! You've been successfully
            registered as a mentor since {formatDate(mentorData.joinDate)}.
          </p>
        </div>

        {getStatusBadge(mentorData.status)}
      </div>
      Current Status Card
      <Card className="mb-8">
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Award className="w-5 h-5 text-blue-600" />
            Your Mentor Profile
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="space-y-3">
              <div className="flex justify-between">
                <span className="font-medium text-gray-600">Hourly Rate:</span>
                <span className="font-semibold">
                  {mentorData.hourlyRate.toFixed(2)} TND
                </span>
              </div>
              <div className="flex justify-between">
                <span className="font-medium text-gray-600">Buffer Time:</span>
                <span className="font-semibold">
                  {mentorData.bufferTime} minutes
                </span>
              </div>
              <div className="flex justify-between">
                <span className="font-medium text-gray-600">Member Since:</span>
                <span className="font-semibold">
                  {formatDate(mentorData.joinDate)}
                </span>
              </div>
            </div>

            <div className="space-y-3">
              <div className="flex justify-between">
                <span className="font-medium text-gray-600">
                  Total Sessions:
                </span>
                <span className="font-semibold">
                  {mentorData.totalSessions}
                </span>
              </div>
              <div className="flex justify-between">
                <span className="font-medium text-gray-600">
                  Total Students:
                </span>
                <span className="font-semibold">
                  {mentorData.totalStudents}
                </span>
              </div>
              <div className="flex justify-between items-center">
                <span className="font-medium text-gray-600">
                  Average Rating:
                </span>
                <div className="flex items-center gap-1">
                  <Star className="w-4 h-4 text-yellow-500 fill-current" />
                  <span className="font-semibold">
                    {mentorData.averageRating}
                  </span>
                </div>
              </div>
            </div>
          </div>

          {mentorData.specialties.length > 0 && (
            <div className="pt-4 border-t">
              <h4 className="font-medium text-gray-600 mb-2">Specialties:</h4>
              <div className="flex flex-wrap gap-2">
                {mentorData.specialties.map((specialty, index) => (
                  <Badge key={index} variant="outline">
                    {specialty}
                  </Badge>
                ))}
              </div>
            </div>
          )}
        </CardContent>
      </Card>
      {/* Quick Stats */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <Card>
          <CardContent className="p-6 text-center">
            <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center mx-auto mb-3">
              <Calendar className="w-6 h-6 text-blue-600" />
            </div>
            <h3 className="font-semibold text-lg">
              {mentorData.upcomingSessions}
            </h3>
            <p className="text-sm text-gray-600">Upcoming Sessions</p>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-6 text-center">
            <div className="w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center mx-auto mb-3">
              <DollarSign className="w-6 h-6 text-green-600" />
            </div>
            <h3 className="font-semibold text-lg">
              {mentorData.totalEarnings.toFixed(2)} TND
            </h3>
            <p className="text-sm text-gray-600">Total Earnings</p>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-6 text-center">
            <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center mx-auto mb-3">
              <Users className="w-6 h-6 text-purple-600" />
            </div>
            <h3 className="font-semibold text-lg">
              {mentorData.totalStudents}
            </h3>
            <p className="text-sm text-gray-600">Students Helped</p>
          </CardContent>
        </Card>
      </div>
      {/* Action Buttons */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
        <Link to="/mentor/dashboard">
          <BarChart3 className="w-5 h-5 mr-2" />
          <div className="text-left">
            <div className="font-semibold">Go to Dashboard</div>
            <div className="text-xs opacity-90">
              Manage your sessions and students
            </div>
          </div>
          <ArrowRight className="w-4 h-4 ml-auto" />
        </Link>

        <Link to="/mentor/settings">
          <Settings className="w-5 h-5 mr-2" />
          <div className="text-left">
            <div className="font-semibold">Mentor Settings</div>
            <div className="text-xs opacity-75">
              Update your profile and preferences
            </div>
          </div>
          <ArrowRight className="w-4 h-4 ml-auto" />
        </Link>
      </div>
      {/* Additional Actions */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg">Quick Actions</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <Button
              variant="ghost"
              className="h-auto p-4 text-left justify-start"
            >
              <Link to="/mentor/sessions/new">
                <Calendar className="w-5 h-5 mr-3 text-blue-600" />
                <div>
                  <div className="font-medium">Schedule Session</div>
                  <div className="text-xs text-gray-600">
                    Add new available time slots
                  </div>
                </div>
              </Link>
            </Button>

            <Button
              variant="ghost"
              className="h-auto p-4 text-left justify-start"
            >
              <Link to={'/mentor/students'}>
                <MessageCircle className="w-5 h-5 mr-3 text-green-600" />
                <div>
                  <div className="font-medium">Message Students</div>
                  <div className="text-xs text-gray-600">
                    Connect with your mentees
                  </div>
                </div>
              </Link>
            </Button>

            <Button
              variant="ghost"
              className="h-auto p-4 text-left justify-start"
            >
              <Link to={'/mentor/earnings'}>
                <DollarSign className="w-5 h-5 mr-3 text-purple-600" />
                <div>
                  <div className="font-medium">View Earnings</div>
                  <div className="text-xs text-gray-600">
                    Track your income and payouts
                  </div>
                </div>
              </Link>
            </Button>
          </div>
        </CardContent>
      </Card>
      {/* Help Section */}
      <div className="text-center mt-8 p-6 bg-gray-50 rounded-lg">
        <h3 className="font-medium text-gray-900 mb-2">
          Need help with your mentor account?
        </h3>
        <p className="text-sm text-gray-600 mb-4">
          If you need to update your mentor information or have any questions,
          our support team is here to help.
        </p>
        <div className="flex flex-col sm:flex-row gap-2 justify-center">
          <Button variant="outline" size="sm">
            <Link to={'/support'}>Contact Support</Link>
          </Button>
          <Button variant="ghost" size="sm">
            <Link to={'/mentor/faq'}>View FAQ</Link>
          </Button>
        </div>
      </div>
    </div>
  );
}

export const Route = createFileRoute(ROUTE_PATHS.TEST.ALREADY)({
  component: RouteComponent,
});

function RouteComponent() {
  return <AlreadyMentorPage />;
}
