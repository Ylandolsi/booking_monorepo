import type { Product } from '@/types/product';

export const mockStore = {
  name: "Sarah's Digital Studio",
  description: 'Premium design resources, coaching calls, and digital products for creative entrepreneurs.',
  profilePicture: 'https://images.unsplash.com/photo-1494790108755-2616b612b830?w=150&h=150&fit=crop&crop=face',
  socialLinks: {
    instagram: 'https://instagram.com/sarahstudio',
    twitter: 'https://twitter.com/sarahstudio',
    website: 'https://sarahstudio.com',
  },
};

export const mockProducts: Product[] = [
  {
    id: '1',
    title: '1-Hour Strategy Call',
    subtitle: 'Get clarity on your creative business direction',
    price: '$199',
    coverImage: 'https://images.unsplash.com/photo-1551836022-deb4988cc6c0?w=400&h=400&fit=crop',
    ctaText: 'Book Now',
    type: 'booking',
    thumbnailMode: 'expanded',
    description: `Join me for a personalized 1-hour strategy session where we'll dive deep into your creative business goals.

What you'll get:
• Clear action plan for your next 90 days
• Personalized strategy recommendations
• Resource list tailored to your needs
• Follow-up email with session notes

Perfect for creative entrepreneurs who are ready to take their business to the next level.`,
    bookingDetails: {
      duration: 60,
      bufferTime: 15,
      timezone: 'PST',
      meetingPlatform: 'zoom',
      meetingLink: 'https://zoom.us/j/strategycall',
      maxAttendees: 1,
      availableSlots: [],
    },
    integrations: [
      {
        id: '1',
        type: 'social',
        platform: 'linkedin',
        url: 'https://linkedin.com/in/sarahstudio',
        icon: '💼',
        label: 'LinkedIn',
      },
    ],
  },
  {
    id: '2',
    title: 'Ultimate Branding Kit',
    subtitle: 'Everything you need to build a stunning brand',
    price: '$97',
    coverImage: 'https://images.unsplash.com/photo-1561070791-2526d30994b5?w=400&h=400&fit=crop',
    ctaText: 'Buy Now',
    type: 'digital',
    thumbnailMode: 'compact',
    description: `A comprehensive branding package that includes everything you need to create a cohesive, professional brand.

What's included:
• 50+ Logo templates (PSD & AI files)
• Color palette generator
• Font pairing guide
• Brand guidelines template
• Social media templates
• Business card templates

All files are easily customizable and include step-by-step instructions.`,
    digitalDetails: {
      downloadLink: 'https://drive.google.com/file/d/branding-kit',
      previewMedia: null,
      downloadFile: null,
    },
    integrations: [
      {
        id: '2',
        type: 'social',
        platform: 'instagram',
        url: 'https://instagram.com/sarahstudio',
        icon: '📷',
        label: 'Instagram',
      },
      {
        id: '3',
        type: 'music',
        platform: 'spotify',
        url: 'https://open.spotify.com/show/design-podcast',
        icon: '🎧',
        label: 'Spotify',
      },
    ],
  },
  {
    id: '3',
    title: 'Quick Design Review',
    subtitle: '30-minute feedback session for your current project',
    price: '$79',
    coverImage: 'https://images.unsplash.com/photo-1586227740560-8cf2732c1531?w=400&h=400&fit=crop',
    ctaText: 'Book Review',
    type: 'booking',
    thumbnailMode: 'expanded',
    description: `Get expert feedback on your current design project in this focused 30-minute session.

What we'll cover:
• Design critique and improvement suggestions
• Color and typography feedback
• User experience recommendations
• Next steps for your project

Bring your work-in-progress and get actionable feedback to make it even better.`,
    bookingDetails: {
      duration: 30,
      bufferTime: 10,
      timezone: 'PST',
      meetingPlatform: 'google-meet',
      meetingLink: 'https://meet.google.com/design-review',
      maxAttendees: 1,
      availableSlots: [],
    },
    integrations: [
      {
        id: '4',
        type: 'custom',
        platform: 'website',
        url: 'https://sarahstudio.com/portfolio',
        icon: '🌐',
        label: 'Portfolio',
      },
    ],
  },
];

export const emptyStore = {
  name: 'My New Store',
  description: "Welcome to my store! I'll be adding products soon.",
  profilePicture: undefined,
  socialLinks: {},
};
