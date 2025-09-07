import { cn } from '@/lib/cn';
import { Link } from '@tanstack/react-router';

import {
  FaLinkedin,
  FaGithub,
  FaTwitter,
  FaInstagram,
  FaFacebook,
  FaYoutube,
  FaGlobe,
  FaDiscord,
  FaTelegram,
} from 'react-icons/fa';

export interface SocialLink {
  platform: string;
  url: string;
  username?: string;
}

export type SocialLinksObject = {
  [key: string]: string | null;
};

interface SocialLinksProps {
  links: SocialLinksObject;
  variant?: 'icons' | 'list';
  size?: 'sm' | 'md' | 'lg';
  className?: string;
}

const socialPlatforms = {
  linkedIn: {
    icon: FaLinkedin,
    color: 'text-blue-600',
    bgColor: 'bg-blue-50 hover:bg-blue-100',
    name: 'LinkedIn',
  },
  github: {
    icon: FaGithub,
    color: 'text-gray-700',
    bgColor: 'bg-gray-50 hover:bg-gray-100',
    name: 'GitHub',
  },
  twitter: {
    icon: FaTwitter,
    color: 'text-blue-500',
    bgColor: 'bg-blue-50 hover:bg-blue-100',
    name: 'Twitter',
  },
  instagram: {
    icon: FaInstagram,
    color: 'text-pink-600',
    bgColor: 'bg-pink-50 hover:bg-pink-100',
    name: 'Instagram',
  },
  facebook: {
    icon: FaFacebook,
    color: 'text-blue-700',
    bgColor: 'bg-blue-50 hover:bg-blue-100',
    name: 'Facebook',
  },
  youtube: {
    icon: FaYoutube,
    color: 'text-red-600',
    bgColor: 'bg-red-50 hover:bg-red-100',
    name: 'YouTube',
  },
  portfolio: {
    icon: FaGlobe,
    color: 'text-gray-600',
    bgColor: 'bg-gray-50 hover:bg-gray-100',
    name: 'Website',
  },
  discord: {
    icon: FaDiscord,
    color: 'text-indigo-600',
    bgColor: 'bg-indigo-50 hover:bg-indigo-100',
    name: 'Discord',
  },
  telegram: {
    icon: FaTelegram,
    color: 'text-blue-500',
    bgColor: 'bg-blue-50 hover:bg-blue-100',
    name: 'Telegram',
  },
};

export function SocialLinks({
  links,
  variant = 'icons',
  size = 'md',
  className = '',
}: SocialLinksProps) {
  const sizeClasses = {
    sm: 'w-4 h-4',
    md: 'w-5 h-5',
    lg: 'w-6 h-6',
  };

  const buttonSizes = {
    sm: 'p-2',
    md: 'p-3',
    lg: 'p-4',
  };

  if (variant === 'icons') {
    return (
      <div className={cn('flex gap-2', className)}>
        {Object.entries(links).map(([key, url], index) => {
          const platform = socialPlatforms[key as keyof typeof socialPlatforms];
          if (!platform || url == null || url == undefined) return null;

          const Icon = platform.icon;

          return (
            <Link
              key={index}
              to={url}
              target="_blank"
              rel="noopener noreferrer"
              className={cn(
                'rounded-lg transition-colors',
                platform.bgColor,
                buttonSizes[size],
              )}
            >
              <Icon className={cn(sizeClasses[size], platform.color)} />
            </Link>
          );
        })}
      </div>
    );
  }

  // List variant
  return (
    <div className={cn('space-y-2', className)}>
      {Object.entries(links).map(([key, url], index) => {
        const platform = socialPlatforms[key as keyof typeof socialPlatforms];
        if (!platform || url == null || url == undefined) return null;

        const Icon = platform.icon;

        return (
          <Link
            key={index}
            to={url}
            target="_blank"
            rel="noopener noreferrer"
            className="flex items-center gap-3 p-2 rounded-lg hover:bg-muted transition-colors"
          >
            <Icon className={cn(sizeClasses[size], platform.color)} />
            <div>
              <div className="font-medium">{platform.name}</div>
            </div>
          </Link>
        );
      })}
    </div>
  );
}
