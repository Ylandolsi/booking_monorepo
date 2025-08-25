import { useState } from 'react';
import { Copy, Check, ExternalLink } from 'lucide-react';
import { LazyImage } from '@/utils';
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
    <div className="group relative flex items-center bg-white hover:bg-gray-50 rounded-2xl shadow-sm hover:shadow-lg border border-gray-100 px-6 py-4 space-x-4 transition-all duration-300">
      <div className="absolute inset-0 rounded-2xl bg-gradient-to-r from-primary/10 to-secondary/10 opacity-0 group-hover:opacity-100 transition-opacity duration-300" />

      <div className="relative z-10 flex items-center space-x-4 flex-1">
        {/* Enhanced Avatar */}
        <div className="relative">
          <LazyImage
            src={avatar}
            placeholder={avatar}
            alt={title}
            className="w-16 h-16 rounded-2xl object-cover ring-2 ring-white shadow-md group-hover:shadow-lg transition-shadow duration-300"
          />
          {/* Status indicator dot */}
          {/* <div className="absolute -bottom-1 -right-1 w-5 h-5 bg-green-500 border-2 border-white rounded-full shadow-sm" /> */}
        </div>

        {/* Content Section */}
        <div className="flex flex-1 flex-col items-start justify-between sm:items-center sm:flex-row">
          <div className="space-y-1">
            <h3 className="font-bold text-gray-900 text-lg group-hover:text-primary transition-colors duration-200">
              {title}
            </h3>
            <p className="text-gray-500 text-sm font-medium flex items-center space-x-1">
              <span>{time}</span>
            </p>
          </div>

          {/* Action Buttons */}
          <div className="flex flex-col mr items-center space-x-3">
            {/* Copy Link Button */}
            <Button
              variant="outline"
              onClick={handleCopy}
              className={`
           w-30
               ounded-xl font-medium transition-all duration-300 
                ${
                  copied
                    ? 'bg-green-50 border-green-200 text-green-700 hover:bg-green-50'
                    : 'bg-white border-gray-200 text-gray-600 hover:bg-gray-50 hover:border-primary hover:text-primary'
                }
                shadow-sm hover:shadow-md
              `}
            >
              {copied ? (
                <>
                  <Check className="w-4 h-4 mr-2" />
                  Copied!
                </>
              ) : (
                <>
                  <Copy className="w-4 h-4 mr-2" />
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
