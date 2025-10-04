import { useState } from 'react';
import { Copy, Check, ExternalLink } from 'lucide-react';
import { LazyImage } from '@/lib';
import { Button } from '@/components';

export function MeetingCard({
  title,
  link,
  time,
  avatar,
  setMeetingLink,
}: {
  title: string;
  link: string;
  time: string;
  avatar: string;
  setMeetingLink: (link: string) => void;
}) {
  const [copied, setCopied] = useState(false);

  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(link);
      setMeetingLink(link);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (e) {
      // Fallback for older browsers
      const textArea = document.createElement('textarea');
      textArea.value = link;
      document.body.appendChild(textArea);
      textArea.select();
      document.execCommand('copy');
      document.body.removeChild(textArea);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    }
  };

  return (
    <div className="group border-border relative flex items-center space-x-4 rounded-2xl border bg-white px-6 py-4 shadow-sm transition-all duration-300 hover:bg-gray-50 hover:shadow-lg">
      <div className="from-secondary/30 to-secondary/10 absolute inset-0 rounded-2xl bg-gradient-to-r opacity-0 transition-opacity duration-300 group-hover:opacity-100" />

      <div className="relative z-10 flex flex-1 items-center space-x-4">
        {/* Enhanced Avatar */}
        <div className="relative">
          <LazyImage
            src={avatar}
            placeholder={avatar}
            alt={title}
            className="h-16 w-16 rounded-full object-cover shadow-md transition-shadow duration-300 group-hover:shadow-lg"
          />
          {/* Status indicator dot */}
          {/* <div className="absolute -bottom-1 -right-1 w-5 h-5 bg-green-500 border-2 border-white rounded-full shadow-sm" /> */}
        </div>

        {/* Content Section */}
        <div className="flex flex-1 flex-col items-start justify-between sm:flex-row sm:items-center">
          <div className="space-y-1">
            <h3 className="group-hover:text-primary text-lg font-bold text-gray-900 transition-colors duration-200">{title}</h3>
            <p className="flex items-center space-x-1 text-sm font-medium text-gray-500">
              <span>{time}</span>
            </p>
          </div>

          {/* Action Buttons */}
          <div className="mr flex flex-col items-center space-x-3">
            {/* Copy Link Button */}
            <Button
              variant="outline"
              onClick={handleCopy}
              className={`ounded-xl w-30 font-medium transition-all duration-300 ${
                copied
                  ? 'border-green-200 bg-green-50 text-green-700 hover:bg-green-50'
                  : 'hover:border-primary hover:text-primary border-gray-200 bg-white text-gray-600 hover:bg-gray-50'
              } shadow-sm hover:shadow-md`}
            >
              {copied ? (
                <>
                  <Check className="mr-2 h-4 w-4" />
                  Copied!
                </>
              ) : (
                <>
                  <Copy className="mr-2 h-4 w-4" />
                  Copy Link
                </>
              )}
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}
