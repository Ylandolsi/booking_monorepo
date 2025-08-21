import { useState } from 'react';
import { ROUTE_PATHS } from '@/config';
import { createFileRoute } from '@tanstack/react-router';
import { Copy } from 'lucide-react';
import { Label } from '@radix-ui/react-label';
import { Input } from '@/components';

export const Route = createFileRoute(ROUTE_PATHS.APP.MEETS.INDEX)({
  component: RouteComponent,
});

function RouteComponent() {
  const [meetingLink, setMeetingLink] = useState('');
  const [copied, setCopied] = useState(false);

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
              <span className="absolute top-0 right-12 text-blue-600 text-sm">
                Copied!
              </span>
            )}
          </div>
        </Label>
      </div>

      {/* Upcoming Meetings */}
      <section>
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-2xl font-semibold text-gray-900">
            Upcoming Meetings
          </h2>
          <div className="flex space-x-2">
            {['Today', 'Tomorrow', 'Next 3 Days', 'Next Week'].map((filter) => (
              <button
                key={filter}
                className={`px-4 py-1 rounded-full text-sm ${
                  filter === 'Today'
                    ? 'bg-muted-foreground/20 font-medium'
                    : ' text-muted-foreground'
                }`}
              >
                {filter}
              </button>
            ))}
          </div>
        </div>
        <div className="space-y-4">
          <MeetingCard
            title="Mentorship Session with Sarah Lee"
            link="meet.connectup.com/sarah.lee"
            time="10:00 AM - 11:00 AM"
            avatar="https://ui-avatars.com/api/?name=Sarah+Lee&background=FFB085&color=fff&size=64"
          />
          <MeetingCard
            title="Career Advice with David Chen"
            link="meet.connectup.com/david.chen"
            time="2:00 PM - 3:00 PM"
            avatar="https://ui-avatars.com/api/?name=David+Chen&background=FFB085&color=fff&size=64"
          />
          <MeetingCard
            title="Project Review with Emily White"
            link="meet.connectup.com/emily.white"
            time="4:00 PM - 5:00 PM"
            avatar="https://ui-avatars.com/api/?name=Emily+White&background=FFB085&color=fff&size=64"
          />
        </div>
      </section>
    </div>
  );
}

function MeetingCard({
  title,
  link,
  time,
  avatar,
}: {
  title: string;
  link: string;
  time: string;
  avatar: string;
}) {
  return (
    <div className="flex items-center bg-white rounded-xl shadow px-4 py-3 space-x-4">
      <img
        src={avatar}
        alt={title}
        className="w-14 h-14 rounded-full object-cover"
      />
      <div>
        <h3 className="font-semibold text-gray-900">{title}</h3>
        <p className="text-blue-600 text-sm">Meeting Link: {link}</p>
        <p className="text-gray-500 text-sm">{time}</p>
      </div>
    </div>
  );
}
