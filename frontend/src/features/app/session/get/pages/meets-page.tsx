import { useState } from 'react';
import { Copy } from 'lucide-react';
import { Label } from '@radix-ui/react-label';
import { Button, ErrorComponenet, Input, LoadingState } from '@/components';
import { MeetingCard } from '@/features/app/session/get/components';
import { useGetSessions } from '@/features/app/session/get/api';
import { formatISODateTime, GenerateTimeZoneId } from '@/utils';

const toDate = {
  All: 600,
  Today: 0,
  Tomorrow: 1,
  // 'Next 3 Days': 3,
  // 'Next Week': 7,
};
export function MeetsPage() {
  const [meetingLink, setMeetingLink] = useState<string>('');
  const [copied, setCopied] = useState<boolean>(false);
  const [upToDate, setUpToDate] = useState<Date | undefined>(undefined);
  const [selectedDuration, setSelectedDuration] = useState<
    keyof typeof toDate | undefined
  >(undefined);
  const timeZoneId = GenerateTimeZoneId();
  // for now we didnt specify limit for the sessions so its undefined !
  const {
    data: sessions,
    error,
    isLoading,
  } = useGetSessions(upToDate, timeZoneId);

  if (isLoading) {
    return <LoadingState type="spinner" message="Loading sessions ..." />;
  }
  if (error) {
    return (
      <ErrorComponenet
        title="Failed to fetch sessions"
        message="Error happened while fetching sessions"
      />
    );
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
      <p className="text-muted-foreground mb-6">
        Manage your meetings and schedule new ones.
      </p>

      {/* Meeting Link */}
      <div className="mb-8">
        <Label className="block font-semibold">
          Meeting Link
          <div className="relative mt-2">
            <Input
              type="text"
              value={meetingLink}
              onChange={(e) => setMeetingLink(e.target.value)}
            />
            <button
              type="button"
              onClick={handleCopy}
              aria-label="Copy meeting link"
              className="absolute top-1/2 right-3 transform -translate-y-1/2 hover:cursor-pointer"
            >
              <Copy size={20} />
            </button>
            {copied && (
              <span className="absolute top-2 right-12 text-primary text-sm">
                Copied!
              </span>
            )}
          </div>
        </Label>
      </div>

      {/* Upcoming Meetings */}
      <section>
        <div className="flex flex-col gap-4  mb-4">
          <h2 className="text-2xl font-semibold text-gray-900">
            Upcoming Meetings
          </h2>
          {/* TODO : for future enable this filter */}
          <div className="flex space-x-2">
            {(Object.keys(toDate) as Array<keyof typeof toDate>).map(
              (filter) => (
                <Button
                  variant="ghost"
                  key={filter}
                  className={`px-4 py-1 rounded-full text-sm ${
                    filter === selectedDuration
                      ? 'bg-muted-foreground/20 font-medium'
                      : 'text-muted-foreground'
                  }`}
                  onClick={() => {
                    const value = toDate[filter];
                    const date = new Date();
                    date.setDate(date.getDate() + value);
                    setUpToDate(date);
                    setSelectedDuration(filter);
                  }}
                >
                  {filter}
                </Button>
              ),
            )}
          </div>
        </div>
        <div className="space-y-4">
          {sessions?.map((session) => {
            let atendeeName =
              [session.mentorFirstName, session.mentorLastName]
                .filter(Boolean)
                .join(' ') || 'Mentor';

            let atendeeProfilePic =
              session.mentorProfilePicture ??
              `https://ui-avatars.com/api/?name=${encodeURIComponent(
                atendeeName,
              )}&background=FFB085&co lor=fff&size=64`;

            if (session.iamMentor) {
              atendeeName =
                [session.menteeFirstName, session.menteeLastName]
                  .filter(Boolean)
                  .join(' ') || 'Mentee';

              atendeeProfilePic =
                session.menteeProfilePicture ??
                // TODO : add fallback for non profile picture
                `https://ui-avatars.com/api/?name=${encodeURIComponent(
                  atendeeName,
                )}&background=FFB085&co lor=fff&size=64`;
            }

            const title = `Session with ${atendeeName}`;
            const link = session.googleMeetLink ?? '';
            const time = formatISODateTime(session.scheduledAt);

            return (
              <MeetingCard
                key={session.id}
                title={title}
                link={link}
                time={time}
                avatar={atendeeProfilePic}
                setMeetingLink={setMeetingLink}
              />
            );
          })}
        </div>
      </section>
    </div>
  );
}
