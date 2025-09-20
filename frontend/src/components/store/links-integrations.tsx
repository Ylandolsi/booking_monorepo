import { useState } from 'react';
import { cn } from '@/lib/cn';

interface Integration {
  id: string;
  type: 'social' | 'music' | 'custom';
  platform: string;
  url: string;
  icon: string;
  label: string;
}

interface LinksIntegrationsProps {
  integrations: Integration[];
  onIntegrationsChange: (integrations: Integration[]) => void;
  className?: string;
}

const PLATFORM_OPTIONS = [
  // Social Media
  { type: 'social', platform: 'instagram', icon: '📷', label: 'Instagram', placeholder: 'https://instagram.com/username' },
  { type: 'social', platform: 'tiktok', icon: '🎵', label: 'TikTok', placeholder: 'https://tiktok.com/@username' },
  { type: 'social', platform: 'youtube', icon: '📺', label: 'YouTube', placeholder: 'https://youtube.com/c/channel' },
  { type: 'social', platform: 'twitter', icon: '🐦', label: 'Twitter/X', placeholder: 'https://twitter.com/username' },
  { type: 'social', platform: 'linkedin', icon: '💼', label: 'LinkedIn', placeholder: 'https://linkedin.com/in/username' },

  // Music & Podcasts
  { type: 'music', platform: 'spotify', icon: '🎧', label: 'Spotify', placeholder: 'https://open.spotify.com/artist/...' },
  { type: 'music', platform: 'apple-music', icon: '🍎', label: 'Apple Music', placeholder: 'https://music.apple.com/artist/...' },
  { type: 'music', platform: 'soundcloud', icon: '☁️', label: 'SoundCloud', placeholder: 'https://soundcloud.com/username' },
  { type: 'music', platform: 'apple-podcasts', icon: '🎙️', label: 'Apple Podcasts', placeholder: 'https://podcasts.apple.com/podcast/...' },

  // Custom
  { type: 'custom', platform: 'website', icon: '🌐', label: 'Website', placeholder: 'https://yourwebsite.com' },
  { type: 'custom', platform: 'custom', icon: '🔗', label: 'Custom Link', placeholder: 'https://your-link.com' },
] as const;

export function LinksIntegrations({ integrations, onIntegrationsChange, className }: LinksIntegrationsProps) {
  const [newIntegration, setNewIntegration] = useState<Partial<Integration>>({});
  const [isAdding, setIsAdding] = useState(false);

  const addIntegration = () => {
    if (newIntegration.platform && newIntegration.url) {
      const platformInfo = PLATFORM_OPTIONS.find((p) => p.platform === newIntegration.platform);
      if (platformInfo) {
        const integration: Integration = {
          id: Date.now().toString(),
          type: platformInfo.type,
          platform: newIntegration.platform,
          url: newIntegration.url,
          icon: platformInfo.icon,
          label: platformInfo.label,
        };

        onIntegrationsChange([...integrations, integration]);
        setNewIntegration({});
        setIsAdding(false);
      }
    }
  };

  const removeIntegration = (id: string) => {
    onIntegrationsChange(integrations.filter((i) => i.id !== id));
  };

  const updateIntegration = (id: string, url: string) => {
    onIntegrationsChange(integrations.map((i) => (i.id === id ? { ...i, url } : i)));
  };

  const groupedIntegrations = integrations.reduce(
    (acc, integration) => {
      if (!acc[integration.type]) {
        acc[integration.type] = [];
      }
      acc[integration.type].push(integration);
      return acc;
    },
    {} as Record<string, Integration[]>,
  );

  return (
    <div className={cn('space-y-6', className)}>
      <div className="text-center mb-6">
        <h3 className="text-lg font-semibold text-foreground mb-2">Links & Integrations</h3>
        <p className="text-muted-foreground">Connect your social media, music platforms, and other links</p>
      </div>

      {/* Existing Integrations */}
      {Object.entries(groupedIntegrations).map(([type, typeIntegrations]) => (
        <div key={type} className="space-y-3">
          <h4 className="text-sm font-semibold text-foreground capitalize flex items-center space-x-2">
            <span>
              {type === 'social' && '👥 Social Media'}
              {type === 'music' && '🎵 Music & Podcasts'}
              {type === 'custom' && '🔗 Custom Links'}
            </span>
          </h4>

          <div className="space-y-2">
            {typeIntegrations.map((integration) => (
              <div key={integration.id} className="flex items-center space-x-3 p-3 bg-muted rounded-lg">
                <span className="text-lg">{integration.icon}</span>
                <div className="flex-1 space-y-1">
                  <div className="text-sm font-medium text-foreground">{integration.label}</div>
                  <input
                    type="url"
                    value={integration.url}
                    onChange={(e) => updateIntegration(integration.id, e.target.value)}
                    className="w-full px-2 py-1 text-xs bg-background border border-border rounded text-foreground focus:outline-none focus:ring-1 focus:ring-ring"
                  />
                </div>
                <button onClick={() => removeIntegration(integration.id)} className="text-muted-foreground hover:text-destructive p-1 rounded">
                  ✕
                </button>
              </div>
            ))}
          </div>
        </div>
      ))}

      {/* Add New Integration */}
      <div className="border-t border-border pt-6">
        {!isAdding ? (
          <button
            onClick={() => setIsAdding(true)}
            className="w-full py-3 px-4 border-2 border-dashed border-border rounded-lg text-muted-foreground hover:border-primary hover:text-primary transition-colors"
          >
            + Add Link or Integration
          </button>
        ) : (
          <div className="space-y-4 p-4 bg-muted rounded-lg">
            <h4 className="text-sm font-semibold text-foreground">Add New Link</h4>

            {/* Platform Selection */}
            <div className="space-y-2">
              <label className="block text-xs font-medium text-foreground">Platform</label>
              <div className="grid grid-cols-2 gap-2 max-h-32 overflow-y-auto">
                {PLATFORM_OPTIONS.map((option) => (
                  <button
                    key={option.platform}
                    type="button"
                    onClick={() => setNewIntegration({ ...newIntegration, platform: option.platform })}
                    className={cn(
                      'flex items-center space-x-2 p-2 rounded border text-left text-xs',
                      newIntegration.platform === option.platform
                        ? 'border-primary bg-primary/5 text-primary'
                        : 'border-border hover:border-primary/50',
                    )}
                  >
                    <span>{option.icon}</span>
                    <span>{option.label}</span>
                  </button>
                ))}
              </div>
            </div>

            {/* URL Input */}
            {newIntegration.platform && (
              <div className="space-y-2">
                <label className="block text-xs font-medium text-foreground">URL</label>
                <input
                  type="url"
                  value={newIntegration.url || ''}
                  onChange={(e) => setNewIntegration({ ...newIntegration, url: e.target.value })}
                  placeholder={PLATFORM_OPTIONS.find((p) => p.platform === newIntegration.platform)?.placeholder}
                  className="w-full px-3 py-2 bg-background border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring text-sm"
                />
              </div>
            )}

            {/* Actions */}
            <div className="flex space-x-2">
              <button
                onClick={addIntegration}
                disabled={!newIntegration.platform || !newIntegration.url}
                className={cn(
                  'flex-1 py-2 px-3 rounded-lg text-sm font-medium transition-all',
                  newIntegration.platform && newIntegration.url
                    ? 'bg-primary text-primary-foreground hover:opacity-90'
                    : 'bg-muted text-muted-foreground cursor-not-allowed',
                )}
              >
                Add Link
              </button>
              <button
                onClick={() => {
                  setIsAdding(false);
                  setNewIntegration({});
                }}
                className="flex-1 py-2 px-3 border border-border rounded-lg text-sm font-medium text-foreground hover:bg-muted transition-colors"
              >
                Cancel
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Preview Note */}
      {integrations.length > 0 && (
        <div className="p-3 bg-primary/5 border border-primary/20 rounded-lg">
          <p className="text-xs text-primary">💡 These links will appear in your store header next to your profile information.</p>
        </div>
      )}
    </div>
  );
}
