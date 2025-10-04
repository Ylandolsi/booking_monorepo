import { useLocation } from '@tanstack/react-router';
import { useEffect } from 'react';
import { toast } from 'sonner';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { useAuth } from '@/api/auth';
import { useAppNavigation } from '@/hooks/use-navigation';
import { Calendar, Video, TrendingUp, Settings, User } from 'lucide-react';
import { GiTeacher } from 'react-icons/gi';
import { useGetSessions } from '@/pages/app/session';

export function HomePage() {
  const location = useLocation();
  const error = new URLSearchParams(location.search).get('error') ?? undefined;
  const { currentUser } = useAuth();
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
        <h1 className="text-foreground text-3xl font-bold tracking-tight">
          {getGreeting()}, {currentUser?.firstName}! 👋
        </h1>
        <p className="text-muted-foreground mt-2">{'Welcome back to your mentoring dashboard'}</p>
      </div>

      {/* Quick Stats */}
      <div className="flex w-full items-center">
        <Card className="w-full">
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Upcoming Sessions</CardTitle>
            <Calendar className="text-muted-foreground h-4 w-4" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{sessionsIsLoading ? '...' : (sessionsData?.length ?? 0)}</div>
            <p className="text-muted-foreground text-xs font-bold">
              {sessionsIsLoading ? 'Loading...' : nextSessionInfo ? `Next session ${nextSessionInfo.timeString}` : 'No upcoming sessions'}
            </p>
          </CardContent>
        </Card>
      </div>

      {/* Main Content Grid */}
      <div className="grid gap-6 xl:grid-cols-3">
        {/* Quick Actions */}
        <div className="space-y-6 xl:col-span-2">
          {/* Actions Card */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <TrendingUp className="h-5 w-5" />
                Quick Actions
              </CardTitle>
            </CardHeader>
            <CardContent className="grid gap-4 sm:grid-cols-2">
              <>
                <Button
                  variant="outline"
                  className="h-auto flex-col items-start justify-start space-y-2 p-4 text-left"
                  onClick={() => nav.goToMentorSetSchedule()}
                >
                  <div className="flex w-full items-center gap-2">
                    <Calendar className="text-primary h-5 w-5" />
                    <span className="font-medium">Set Availability</span>
                  </div>
                  <span className="text-muted-foreground text-sm">Update your available time slots</span>
                </Button>
                <Button
                  variant="outline"
                  className="h-auto flex-col items-start justify-start space-y-2 p-4 text-left"
                  onClick={() => nav.goToMeets()}
                >
                  <div className="flex w-full items-center gap-2">
                    <Video className="text-primary h-5 w-5" />
                    <span className="font-medium">View Sessions</span>
                  </div>
                  <span className="text-muted-foreground text-sm">Manage your upcoming meetings</span>
                </Button>
              </>
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
                <div className="bg-primary/10 flex h-12 w-12 items-center justify-center rounded-full">
                  <User className="text-primary h-6 w-6" />
                </div>
                <div className="min-w-0 flex-1">
                  <p className="font-medium">
                    {currentUser?.firstName} {currentUser?.lastName}
                  </p>
                  <p className="text-muted-foreground truncate text-sm">{currentUser?.email}</p>
                </div>
              </div>
              <div className="flex items-center gap-2">
                <Badge variant="secondary" className="gap-1">
                  <GiTeacher className="h-3 w-3" />
                  Mentor
                </Badge>
              </div>
              <Button variant="outline" size="sm" className="w-full" onClick={() => nav.goToProfile(currentUser?.slug || '')}>
                <Settings className="mr-2 h-4 w-4" />
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
