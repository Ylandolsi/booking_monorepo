import { useState } from 'react';
import { Copy, Filter } from 'lucide-react';
import { Label } from '@radix-ui/react-label';
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  ErrorComponenet,
  Input,
  LoadingState,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components';
import { MeetingCard } from '@/features/app/session/get/components';
import { useGetSessions } from '@/features/app/session/get/api';
import { formatISODateTime, GenerateTimeZoneId } from '@/utils';
import { useTimeFilter, type TimeFilter } from '@/hooks/use-time-filter';
import { FALLBACK_PROFILE_PICTURE } from '@/lib';

export function MeetsPage() {
  const [meetingLink, setMeetingLink] = useState<string>('');
  const [copied, setCopied] = useState<boolean>(false);

  const { upToDate, timeFilter, setTimeStatus } = useTimeFilter();

  const timeZoneId = GenerateTimeZoneId();
  // for now we didnt specify limit for the sessions so its undefined !
  const { data: sessions, error, isLoading } = useGetSessions(upToDate, timeZoneId);

  if (isLoading) {
    return <LoadingState type="spinner" message="Loading sessions ..." />;
  }
  if (error) {
    return <ErrorComponenet title="Failed to fetch sessions" message="Error happened while fetching sessions" />;
  }

  const handleCopy = () => {
    navigator.clipboard.writeText(meetingLink);
    setCopied(true);
    setTimeout(() => setCopied(false), 1000);
  };

  return (
    <div>
      {/* Main Content */}
      <h1 className="text-3xl font-bold mb-2">Meetings</h1>
      <p className="text-muted-foreground mb-6">Manage your meetings and schedule new ones.</p>

      {/* Meeting Link */}
      <div className="mb-8">
        <Label className="block font-semibold">
          Meeting Link
          <div className="relative mt-2">
            <Input type="text" value={meetingLink} onChange={(e) => setMeetingLink(e.target.value)} />
            <button
              type="button"
              onClick={handleCopy}
              aria-label="Copy meeting link"
              className="absolute top-1/2 right-3 transform -translate-y-1/2 hover:cursor-pointer"
            >
              <Copy size={20} />
            </button>
            {copied && <span className="absolute top-2 right-12 text-primary text-sm">Copied!</span>}
          </div>
        </Label>
      </div>

      {/* Upcoming Meetings */}
      <h2 className="text-2xl font-semibold text-gray-900 mb-8">Upcoming Meetings</h2>

      <section className="space-y-5">
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Filter className="h-5 w-5 text-muted-foreground" />
                <CardTitle>Filters</CardTitle>
              </div>
            </div>
          </CardHeader>
          <CardContent>
            <div className="flex flex-col gap-4  mb-4">
              <div className="space-y-2">
                <label className="text-sm font-medium">Time Period</label>
                <Select value={timeFilter} onValueChange={(value: TimeFilter) => setTimeStatus(value)}>
                  <SelectTrigger className="w-[180px]">
                    <SelectValue placeholder="Select time period" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="All">All Time</SelectItem>
                    <SelectItem value="NextHour">Next Hour</SelectItem>
                    <SelectItem value="Next24Hours">Next 24 Hours </SelectItem>
                    <SelectItem value="Next3Days">Next 3 Days</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>
          </CardContent>
        </Card>
        <div className="space-y-4">
          {sessions?.map((session) => {
            let atendeeName = [session.mentorFirstName, session.mentorLastName].filter(Boolean).join(' ') || 'Mentor';

            let atendeeProfilePic = session.mentorProfilePicture ?? FALLBACK_PROFILE_PICTURE;

            if (session.iamMentor) {
              atendeeName = [session.menteeFirstName, session.menteeLastName].filter(Boolean).join(' ') || 'Mentee';

              atendeeProfilePic = session.menteeProfilePicture ?? FALLBACK_PROFILE_PICTURE;
            }

            const title = `Session with ${atendeeName}`;
            const link = session.googleMeetLink ?? '';
            const time = formatISODateTime(session.scheduledAt);

            return <MeetingCard key={session.id} title={title} link={link} time={time} avatar={atendeeProfilePic} setMeetingLink={setMeetingLink} />;
          })}
        </div>
      </section>
    </div>
  );
}
