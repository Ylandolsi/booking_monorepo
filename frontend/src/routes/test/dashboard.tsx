import { createFileRoute } from '@tanstack/react-router';
import React, { useState } from 'react';
import { useUser } from '@/features/auth';
import { ROUTE_PATHS } from '@/config/routes';
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Badge,
  Avatar,
  AvatarFallback,
  AvatarImage,
  Progress,
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from '@/components/ui';
import {
  Calendar,
  Clock,
  DollarSign,
  Star,
  Users,
  BookOpen,
  Settings,
  TrendingUp,
  MessageCircle,
  Award,
  Edit3,
} from 'lucide-react';

export const Route = createFileRoute(ROUTE_PATHS.TEST.DASHBOARD)({
  component: RouteComponent,
});

function RouteComponent() {
  return <MentorDashboardPage />;
}

// Mock data - replace with real API calls
const mockStats = {
  totalSessions: 42,
  totalEarnings: 2450.0,
  averageRating: 4.8,
  totalStudents: 28,
  upcomingSessions: 3,
  completedSessions: 39,
  monthlyEarnings: 850.0,
  weeklyHours: 12,
};

const mockUpcomingSessions = [
  {
    id: 1,
    studentName: 'Ahmed Ben Ali',
    studentAvatar: '/api/placeholder/32/32',
    subject: 'React Development',
    date: '2024-08-10',
    time: '14:00',
    duration: 60,
    type: 'video',
  },
  {
    id: 2,
    studentName: 'Fatma Khelifi',
    studentAvatar: '/api/placeholder/32/32',
    subject: 'JavaScript Fundamentals',
    date: '2024-08-10',
    time: '16:30',
    duration: 45,
    type: 'video',
  },
  {
    id: 3,
    studentName: 'Mohamed Trabelsi',
    studentAvatar: '/api/placeholder/32/32',
    subject: 'Node.js Backend',
    date: '2024-08-11',
    time: '10:00',
    duration: 90,
    type: 'video',
  },
];

const mockRecentSessions = [
  {
    id: 1,
    studentName: 'Sarah Mansouri',
    subject: 'Python Basics',
    date: '2024-08-08',
    rating: 5,
    earnings: 45.0,
    feedback: 'Excellent explanation! Very patient and helpful.',
  },
  {
    id: 2,
    studentName: 'Karim Bouzid',
    subject: 'Data Structures',
    date: '2024-08-07',
    rating: 4,
    earnings: 60.0,
    feedback: 'Good session, would recommend.',
  },
  {
    id: 3,
    studentName: 'Amina Jebali',
    subject: 'Web Development',
    date: '2024-08-06',
    rating: 5,
    earnings: 75.0,
    feedback: 'Amazing mentor! Learned so much in one session.',
  },
];

export function MentorDashboardPage() {
  //   const { data: user } = useUser();
  const user = {};
  const [activeTab, setActiveTab] = useState('overview');

  // Mock loading states - replace with real hooks
  const isLoading = false;
  const stats = mockStats;
  const upcomingSessions = mockUpcomingSessions;
  const recentSessions = mockRecentSessions;

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-[400px]">
        <div className="text-center space-y-2">
          <div className="w-8 h-8 border-4 border-blue-600 border-t-transparent rounded-full animate-spin mx-auto"></div>
          <p className="text-sm text-gray-600">
            Loading your mentor dashboard...
          </p>
        </div>
      </div>
    );
  }

  const formatDate = (dateStr: string) => {
    return new Date(dateStr).toLocaleDateString('en-US', {
      weekday: 'short',
      month: 'short',
      day: 'numeric',
    });
  };

  const getInitials = (name: string) => {
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase();
  };

  return (
    <div className="container mx-auto py-8 px-4 space-y-8">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div className="space-y-1">
          <h1 className="text-3xl font-bold">Welcome back, {'Mentor'}!</h1>
          <p className="text-gray-600">
            Manage your mentoring sessions and track your progress
          </p>
        </div>
        <div className="flex gap-2">
          <Button variant="outline" size="sm">
            <Settings className="w-4 h-4 mr-2" />
            Settings
          </Button>
          <Button size="sm">
            <Edit3 className="w-4 h-4 mr-2" />
            Edit Profile
          </Button>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <Card>
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div className="space-y-1">
                <p className="text-sm font-medium text-gray-600">
                  Total Sessions
                </p>
                <p className="text-2xl font-bold">{stats.totalSessions}</p>
              </div>
              <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
                <BookOpen className="w-6 h-6 text-blue-600" />
              </div>
            </div>
            <div className="flex items-center mt-4 text-sm">
              <TrendingUp className="w-4 h-4 text-green-500 mr-1" />
              <span className="text-green-600">+12% from last month</span>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div className="space-y-1">
                <p className="text-sm font-medium text-gray-600">
                  Total Earnings
                </p>
                <p className="text-2xl font-bold">
                  {stats.totalEarnings.toFixed(2)} TND
                </p>
              </div>
              <div className="w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center">
                <DollarSign className="w-6 h-6 text-green-600" />
              </div>
            </div>
            <div className="flex items-center mt-4 text-sm">
              <span className="text-gray-600">
                This month: {stats.monthlyEarnings.toFixed(2)} TND
              </span>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div className="space-y-1">
                <p className="text-sm font-medium text-gray-600">
                  Average Rating
                </p>
                <div className="flex items-center gap-1">
                  <p className="text-2xl font-bold">{stats.averageRating}</p>
                  <Star className="w-5 h-5 text-yellow-500 fill-current" />
                </div>
              </div>
              <div className="w-12 h-12 bg-yellow-100 rounded-lg flex items-center justify-center">
                <Award className="w-6 h-6 text-yellow-600" />
              </div>
            </div>
            <div className="flex items-center mt-4 text-sm">
              <span className="text-gray-600">
                From {stats.completedSessions} sessions
              </span>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div className="space-y-1">
                <p className="text-sm font-medium text-gray-600">
                  Total Students
                </p>
                <p className="text-2xl font-bold">{stats.totalStudents}</p>
              </div>
              <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center">
                <Users className="w-6 h-6 text-purple-600" />
              </div>
            </div>
            <div className="flex items-center mt-4 text-sm">
              <span className="text-gray-600">Active learners</span>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Main Content Tabs */}
      <Tabs
        value={activeTab}
        onValueChange={setActiveTab}
        className="space-y-6"
      >
        <TabsList className="grid w-full grid-cols-4">
          <TabsTrigger value="overview">Overview</TabsTrigger>
          <TabsTrigger value="sessions">Sessions</TabsTrigger>
          <TabsTrigger value="students">Students</TabsTrigger>
          <TabsTrigger value="earnings">Earnings</TabsTrigger>
        </TabsList>

        {/* Overview Tab */}
        <TabsContent value="overview" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            {/* Upcoming Sessions */}
            <Card>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-base font-medium">
                  Upcoming Sessions
                </CardTitle>
                <Badge variant="secondary">{stats.upcomingSessions}</Badge>
              </CardHeader>
              <CardContent className="space-y-4">
                {upcomingSessions.map((session) => (
                  <div
                    key={session.id}
                    className="flex items-center justify-between p-3 bg-gray-50 rounded-lg"
                  >
                    <div className="flex items-center gap-3">
                      <Avatar className="w-8 h-8">
                        <AvatarImage src={session.studentAvatar} />
                        <AvatarFallback className="text-xs">
                          {getInitials(session.studentName)}
                        </AvatarFallback>
                      </Avatar>
                      <div className="space-y-1">
                        <p className="text-sm font-medium">
                          {session.studentName}
                        </p>
                        <p className="text-xs text-gray-600">
                          {session.subject}
                        </p>
                      </div>
                    </div>
                    <div className="text-right">
                      <div className="flex items-center gap-1 text-xs text-gray-600 mb-1">
                        <Calendar className="w-3 h-3" />
                        {formatDate(session.date)}
                      </div>
                      <div className="flex items-center gap-1 text-xs text-gray-600">
                        <Clock className="w-3 h-3" />
                        {session.time} ({session.duration}min)
                      </div>
                    </div>
                  </div>
                ))}
                <Button variant="outline" className="w-full" size="sm">
                  <Calendar className="w-4 h-4 mr-2" />
                  View All Sessions
                </Button>
              </CardContent>
            </Card>

            {/* Recent Activity */}
            <Card>
              <CardHeader>
                <CardTitle className="text-base font-medium">
                  Recent Sessions
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                {recentSessions.map((session) => (
                  <div
                    key={session.id}
                    className="flex items-center justify-between p-3 bg-gray-50 rounded-lg"
                  >
                    <div className="space-y-1">
                      <p className="text-sm font-medium">
                        {session.studentName}
                      </p>
                      <p className="text-xs text-gray-600">{session.subject}</p>
                      <div className="flex items-center gap-1">
                        {Array.from({ length: 5 }).map((_, i) => (
                          <Star
                            key={i}
                            className={`w-3 h-3 ${
                              i < session.rating
                                ? 'text-yellow-500 fill-current'
                                : 'text-gray-300'
                            }`}
                          />
                        ))}
                      </div>
                    </div>
                    <div className="text-right">
                      <p className="text-sm font-medium text-green-600">
                        +{session.earnings.toFixed(2)} TND
                      </p>
                      <p className="text-xs text-gray-600">
                        {formatDate(session.date)}
                      </p>
                    </div>
                  </div>
                ))}
                <Button variant="outline" className="w-full" size="sm">
                  <MessageCircle className="w-4 h-4 mr-2" />
                  View All Feedback
                </Button>
              </CardContent>
            </Card>
          </div>

          {/* Weekly Progress */}
          <Card>
            <CardHeader>
              <CardTitle className="text-base font-medium">
                This Week's Progress
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <div className="space-y-2">
                  <div className="flex justify-between text-sm">
                    <span>Hours Mentored</span>
                    <span>{stats.weeklyHours}/20</span>
                  </div>
                  <Progress value={(stats.weeklyHours / 20) * 100} />
                </div>
                <div className="space-y-2">
                  <div className="flex justify-between text-sm">
                    <span>Sessions Completed</span>
                    <span>8/10</span>
                  </div>
                  <Progress value={80} />
                </div>
                <div className="space-y-2">
                  <div className="flex justify-between text-sm">
                    <span>Weekly Goal</span>
                    <span>85%</span>
                  </div>
                  <Progress value={85} />
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Sessions Tab */}
        <TabsContent value="sessions" className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Session Management</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-center py-8 text-gray-500">
                <Calendar className="w-12 h-12 mx-auto mb-4 text-gray-400" />
                <p className="text-lg font-medium mb-2">
                  Session management coming soon
                </p>
                <p className="text-sm">
                  Detailed session scheduling and management tools
                </p>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Students Tab */}
        <TabsContent value="students" className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Student Management</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-center py-8 text-gray-500">
                <Users className="w-12 h-12 mx-auto mb-4 text-gray-400" />
                <p className="text-lg font-medium mb-2">
                  Student directory coming soon
                </p>
                <p className="text-sm">
                  Manage your students and track their progress
                </p>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Earnings Tab */}
        <TabsContent value="earnings" className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Earnings Analytics</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-center py-8 text-gray-500">
                <DollarSign className="w-12 h-12 mx-auto mb-4 text-gray-400" />
                <p className="text-lg font-medium mb-2">
                  Detailed earnings coming soon
                </p>
                <p className="text-sm">Track your income and payment history</p>
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
