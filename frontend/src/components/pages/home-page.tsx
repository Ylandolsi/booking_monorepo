import { useLocation } from '@tanstack/react-router';
import { useEffect } from 'react';
import { toast } from 'sonner';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { useAuth } from '@/features/auth/hooks';
import { useMentor } from '@/hooks/use-mentor';
import { useAppNavigation } from '@/hooks/use-navigation';
import { Calendar, BookOpen, Video, TrendingUp, Settings, User } from 'lucide-react';
import { GiTeacher } from 'react-icons/gi';
import { useGetSessions } from '@/features/app/session';

export function HomePage() {
  const location = useLocation();
  const error = new URLSearchParams(location.search).get('error') ?? undefined;
  const { currentUser } = useAuth();
  const { isMentor } = useMentor();
  const nav = useAppNavigation();

  const { data: sessionsData, isLoading: sessionsIsLoading } = useGetSessions();

  useEffect(() => {
    if (error) {
      toast.error(error);
    }
  }, [error]);

  const currentHour = new Date().getHours();
  const getGreeting = () => {
    if (currentHour < 12) return 'Good morning';
    if (currentHour < 18) return 'Good afternoon';
    return 'Good evening';
  };

  // Helper to get the next session and time until it starts
  const getNextSessionInfo = () => {
    if (!sessionsData || !Array.isArray(sessionsData) || sessionsData.length === 0) return null;
    const now = new Date();
    const next = sessionsData[0];
    const startTimeOfSession = new Date(next.scheduledAt);
    const diffMs = startTimeOfSession.getTime() - now.getTime();
    const diffMins = Math.round(diffMs / 60000);
    let timeString = '';
    if (diffMins < 60) {
      timeString = `in ${diffMins} minute${diffMins !== 1 ? 's' : ''}`;
    } else if (diffMins < 1440) {
      const hours = Math.floor(diffMins / 60);
      const mins = diffMins % 60;
      timeString = `in ${hours} hour${hours !== 1 ? 's' : ''}${mins > 0 ? ` ${mins} min` : ''}`;
    } else {
      const days = Math.floor(diffMins / 1440);
      timeString = `in ${days} day${days !== 1 ? 's' : ''}`;
    }
    return { next, timeString };
  };

  const nextSessionInfo = getNextSessionInfo();

  return (
    <div className="space-y-8">
      {/* Welcome Header */}
      <div className="">
        <h1 className="text-3xl font-bold tracking-tight text-foreground">
          {getGreeting()}, {currentUser?.firstName}! 👋
        </h1>
        <p className="mt-2 text-muted-foreground">{isMentor ? 'Welcome back to your mentoring dashboard' : 'Ready to learn something new today?'}</p>
      </div>

      {/* Quick Stats */}
      <div className="flex w-full items-center">
        <Card className="w-full">
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Upcoming Sessions</CardTitle>
            <Calendar className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{sessionsIsLoading ? '...' : (sessionsData?.length ?? 0)}</div>
            <p className="text-xs text-muted-foreground font-bold">
              {sessionsIsLoading ? 'Loading...' : nextSessionInfo ? `Next session ${nextSessionInfo.timeString}` : 'No upcoming sessions'}
            </p>
          </CardContent>
        </Card>
      </div>

      {/* Main Content Grid */}
      <div className="grid gap-6 xl:grid-cols-3">
        {/* Quick Actions */}
        <div className="xl:col-span-2 space-y-6">
          {/* Actions Card */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <TrendingUp className="h-5 w-5" />
                Quick Actions
              </CardTitle>
            </CardHeader>
            <CardContent className="grid gap-4 sm:grid-cols-2">
              {isMentor ? (
                <>
                  <Button
                    variant="outline"
                    className="h-auto p-4 text-left justify-start flex-col items-start space-y-2"
                    onClick={() => nav.goToMentorSetSchedule()}
                  >
                    <div className="flex items-center gap-2 w-full">
                      <Calendar className="h-5 w-5 text-primary" />
                      <span className="font-medium">Set Availability</span>
                    </div>
                    <span className="text-sm text-muted-foreground">Update your available time slots</span>
                  </Button>
                  <Button
                    variant="outline"
                    className="h-auto p-4 text-left justify-start flex-col items-start space-y-2"
                    onClick={() => nav.goToMeets()}
                  >
                    <div className="flex items-center gap-2 w-full">
                      <Video className="h-5 w-5 text-primary" />
                      <span className="font-medium">View Sessions</span>
                    </div>
                    <span className="text-sm text-muted-foreground">Manage your upcoming meetings</span>
                  </Button>
                </>
              ) : (
                <>
                  <Button
                    variant="outline"
                    className="h-auto p-4 text-left justify-start flex-col items-start space-y-2"
                    onClick={() => nav.goToMentorBecome()}
                  >
                    <div className="flex items-center gap-2 w-full">
                      <GiTeacher className="h-5 w-5 text-primary" />
                      <span className="font-medium">Become a Mentor</span>
                    </div>
                    <span className="text-sm text-muted-foreground">Share your expertise with others</span>
                  </Button>
                  <Button
                    variant="outline"
                    className="h-auto p-4 text-left justify-start flex-col items-start space-y-2"
                    onClick={() => nav.goToMeets()}
                  >
                    <div className="flex items-center gap-2 w-full">
                      <BookOpen className="h-5 w-5 text-primary" />
                      <span className="font-medium">Find Mentors</span>
                    </div>
                    <span className="text-sm text-muted-foreground">Discover experts in your field</span>
                  </Button>
                </>
              )}
            </CardContent>
          </Card>
        </div>

        {/* Sidebar Content */}
        <div className="space-y-6">
          {/* Profile Card */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <User className="h-5 w-5" />
                Your Profile
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex items-center gap-3">
                <div className="h-12 w-12 rounded-full bg-primary/10 flex items-center justify-center">
                  <User className="h-6 w-6 text-primary" />
                </div>
                <div className="flex-1 min-w-0">
                  <p className="font-medium">
                    {currentUser?.firstName} {currentUser?.lastName}
                  </p>
                  <p className="text-sm text-muted-foreground truncate">{currentUser?.email}</p>
                </div>
              </div>
              <div className="flex items-center gap-2">
                {isMentor ? (
                  <Badge variant="secondary" className="gap-1">
                    <GiTeacher className="h-3 w-3" />
                    Mentor
                  </Badge>
                ) : (
                  <Badge variant="outline">Student</Badge>
                )}
              </div>
              <Button variant="outline" size="sm" className="w-full" onClick={() => nav.goToProfile(currentUser?.slug || '')}>
                <Settings className="h-4 w-4 mr-2" />
                Edit Profile
              </Button>
            </CardContent>
          </Card>

          {/* Upcoming Sessions */}
          {/* <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Calendar className="h-5 w-5" />
                Upcoming Sessions
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-3">
              <div className="rounded-lg border p-3">
                <div className="flex items-center justify-between">
                  <div>
                    <p className="font-medium text-sm">React Development</p>
                    <p className="text-xs text-muted-foreground">
                      with Sarah Johnson
                    </p>
                  </div>
                  <Badge variant="outline" className="text-xs">
                    Today 2:00 PM
                  </Badge>
                </div>
              </div>
              <div className="rounded-lg border p-3">
                <div className="flex items-center justify-between">
                  <div>
                    <p className="font-medium text-sm">System Design</p>
                    <p className="text-xs text-muted-foreground">
                      with Mike Chen
                    </p>
                  </div>
                  <Badge variant="outline" className="text-xs">
                    Tomorrow 10:00 AM
                  </Badge>
                </div>
              </div>
              <Button
                variant="ghost"
                size="sm"
                className="w-full text-sm"
                onClick={() => nav.goToMeets()}
              >
                View All Sessions
                <ArrowRight className="h-3 w-3 ml-1" />
              </Button>
            </CardContent>
          </Card> */}
        </div>
      </div>
    </div>
  );
}
