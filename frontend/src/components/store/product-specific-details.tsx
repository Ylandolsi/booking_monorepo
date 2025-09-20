import { useState } from 'react';
import { cn } from '@/lib/cn';

interface DigitalProductDetails {
  downloadFile?: File | string | null;
  downloadLink?: string;
  previewMedia?: File | string | null;
}

interface BookingDetails {
  duration: number; // in minutes
  bufferTime: number; // in minutes
  timezone: string;
  meetingLink?: string;
  meetingPlatform: 'zoom' | 'google-meet' | 'custom';
  maxAttendees: number;
  availableSlots: Array<{
    day: string;
    startTime: string;
    endTime: string;
  }>;
}

interface ProductSpecificDetailsProps {
  productType: 'digital' | 'booking';
  digitalDetails?: DigitalProductDetails;
  bookingDetails?: BookingDetails;
  onDigitalDetailsChange?: (details: DigitalProductDetails) => void;
  onBookingDetailsChange?: (details: BookingDetails) => void;
  className?: string;
}

export function ProductSpecificDetails({
  productType,
  digitalDetails = {},
  bookingDetails = {
    duration: 30,
    bufferTime: 15,
    timezone: 'UTC',
    meetingPlatform: 'zoom',
    maxAttendees: 1,
    availableSlots: [],
  },
  onDigitalDetailsChange,
  onBookingDetailsChange,
  className,
}: ProductSpecificDetailsProps) {
  const [digitalPreview, setDigitalPreview] = useState<string | null>(null);

  const handleDigitalFileChange = (field: keyof DigitalProductDetails, file: File | null) => {
    const newDetails = { ...digitalDetails, [field]: file };
    onDigitalDetailsChange?.(newDetails);

    if (field === 'previewMedia' && file) {
      const reader = new FileReader();
      reader.onload = (e) => setDigitalPreview(e.target?.result as string);
      reader.readAsDataURL(file);
    }
  };

  const handleBookingChange = (field: keyof BookingDetails, value: any) => {
    const newDetails = { ...bookingDetails, [field]: value };
    onBookingDetailsChange?.(newDetails);
  };

  if (productType === 'digital') {
    return (
      <div className={cn('space-y-6', className)}>
        <div className="text-center mb-6">
          <h3 className="text-lg font-semibold text-foreground mb-2">Digital Product Settings</h3>
          <p className="text-muted-foreground">Configure file downloads and preview media</p>
        </div>

        {/* Download File */}
        <div className="space-y-2">
          <label className="block text-sm font-medium text-foreground">Download File</label>
          <div className="border-2 border-dashed border-border rounded-lg p-6 text-center">
            <div className="mb-4">
              <div className="text-3xl mb-2">📁</div>
              <p className="text-sm text-muted-foreground mb-2">Upload the file customers will download</p>
            </div>
            <label className="cursor-pointer">
              <input
                type="file"
                onChange={(e) => handleDigitalFileChange('downloadFile', e.target.files?.[0] || null)}
                className="hidden"
                accept=".pdf,.zip,.mp4,.mp3,.png,.jpg,.jpeg,.doc,.docx,.txt"
              />
              <span className="inline-flex items-center px-4 py-2 bg-primary text-primary-foreground rounded-lg hover:opacity-90 transition-opacity">
                Choose File
              </span>
            </label>
            {typeof digitalDetails.downloadFile === 'string' && (
              <p className="text-xs text-muted-foreground mt-2">Current: {digitalDetails.downloadFile.split('/').pop()}</p>
            )}
          </div>
        </div>

        {/* Download Link */}
        <div className="space-y-2">
          <label className="block text-sm font-medium text-foreground">Download Link (Optional)</label>
          <input
            type="url"
            value={digitalDetails.downloadLink || ''}
            onChange={(e) => onDigitalDetailsChange?.({ ...digitalDetails, downloadLink: e.target.value })}
            placeholder="https://drive.google.com/file/..."
            className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
          />
          <p className="text-xs text-muted-foreground">Alternative to file upload. Use for large files or external storage.</p>
        </div>

        {/* Preview Media */}
        <div className="space-y-2">
          <label className="block text-sm font-medium text-foreground">Preview Media (Optional)</label>
          <div className="flex items-start space-x-4">
            <div className="w-24 h-24 rounded-lg overflow-hidden bg-muted flex items-center justify-center border border-border">
              {digitalPreview ? (
                <img src={digitalPreview} alt="Preview" className="w-full h-full object-cover" />
              ) : (
                <span className="text-muted-foreground text-xs text-center">No preview</span>
              )}
            </div>
            <div className="flex-1">
              <label className="cursor-pointer">
                <input
                  type="file"
                  accept="image/*"
                  onChange={(e) => handleDigitalFileChange('previewMedia', e.target.files?.[0] || null)}
                  className="hidden"
                />
                <span className="inline-flex items-center px-4 py-2 bg-secondary text-secondary-foreground rounded-lg hover:bg-secondary/80 transition-colors">
                  Choose Preview Image
                </span>
              </label>
              <p className="text-xs text-muted-foreground mt-1">Show customers a preview of what they'll download</p>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Booking-specific details
  return (
    <div className={cn('space-y-6', className)}>
      <div className="text-center mb-6">
        <h3 className="text-lg font-semibold text-foreground mb-2">Booking Settings</h3>
        <p className="text-muted-foreground">Configure scheduling and meeting details</p>
      </div>

      {/* Duration & Buffer */}
      <div className="grid grid-cols-2 gap-4">
        <div className="space-y-2">
          <label className="block text-sm font-medium text-foreground">Duration (minutes)</label>
          <input
            type="number"
            value={bookingDetails.duration}
            onChange={(e) => handleBookingChange('duration', parseInt(e.target.value) || 30)}
            className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground focus:outline-none focus:ring-2 focus:ring-ring"
            min="15"
            max="480"
            step="15"
          />
        </div>
        <div className="space-y-2">
          <label className="block text-sm font-medium text-foreground">Buffer Time (minutes)</label>
          <input
            type="number"
            value={bookingDetails.bufferTime}
            onChange={(e) => handleBookingChange('bufferTime', parseInt(e.target.value) || 15)}
            className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground focus:outline-none focus:ring-2 focus:ring-ring"
            min="0"
            max="60"
            step="5"
          />
        </div>
      </div>

      {/* Meeting Platform */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Meeting Platform</label>
        <div className="grid grid-cols-3 gap-2">
          {(['zoom', 'google-meet', 'custom'] as const).map((platform) => (
            <button
              key={platform}
              type="button"
              onClick={() => handleBookingChange('meetingPlatform', platform)}
              className={cn(
                'p-3 rounded-lg border-2 transition-all text-center',
                bookingDetails.meetingPlatform === platform ? 'border-primary bg-primary/5 text-primary' : 'border-border hover:border-primary/50',
              )}
            >
              <div className="text-lg mb-1">
                {platform === 'zoom' && '📹'}
                {platform === 'google-meet' && '🎥'}
                {platform === 'custom' && '🔗'}
              </div>
              <div className="text-xs font-medium">
                {platform === 'zoom' && 'Zoom'}
                {platform === 'google-meet' && 'Google Meet'}
                {platform === 'custom' && 'Custom Link'}
              </div>
            </button>
          ))}
        </div>
      </div>

      {/* Meeting Link */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Meeting Link</label>
        <input
          type="url"
          value={bookingDetails.meetingLink || ''}
          onChange={(e) => handleBookingChange('meetingLink', e.target.value)}
          placeholder={
            bookingDetails.meetingPlatform === 'zoom'
              ? 'https://zoom.us/j/...'
              : bookingDetails.meetingPlatform === 'google-meet'
                ? 'https://meet.google.com/...'
                : 'https://your-meeting-link.com'
          }
          className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
        />
      </div>

      {/* Max Attendees */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Maximum Attendees</label>
        <input
          type="number"
          value={bookingDetails.maxAttendees}
          onChange={(e) => handleBookingChange('maxAttendees', parseInt(e.target.value) || 1)}
          className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground focus:outline-none focus:ring-2 focus:ring-ring"
          min="1"
          max="50"
        />
        <p className="text-xs text-muted-foreground">Set to 1 for private sessions, higher for group calls</p>
      </div>

      {/* Availability Placeholder */}
      <div className="p-6 bg-muted rounded-lg text-center">
        <div className="text-2xl mb-2">📅</div>
        <h4 className="font-medium text-foreground mb-2">Availability Scheduler</h4>
        <p className="text-sm text-muted-foreground">Advanced scheduling system coming soon</p>
      </div>
    </div>
  );
}
