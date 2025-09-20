import { cn } from '@/lib/cn';

interface ThumbnailModeSelectorProps {
  value: 'compact' | 'expanded';
  onChange: (mode: 'compact' | 'expanded') => void;
  className?: string;
}

export function ThumbnailModeSelector({ value, onChange, className }: ThumbnailModeSelectorProps) {
  const options = [
    {
      value: 'compact' as const,
      label: 'Compact Icon',
      description: 'Small icon next to title',
      preview: (
        <div className="w-full bg-muted rounded-lg p-2 flex items-center space-x-2">
          <div className="w-4 h-4 bg-primary/20 rounded flex-shrink-0" />
          <div className="flex-1 space-y-1">
            <div className="h-2 bg-foreground/20 rounded w-3/4" />
            <div className="h-1.5 bg-muted-foreground/20 rounded w-1/2" />
          </div>
        </div>
      ),
    },
    {
      value: 'expanded' as const,
      label: 'Expanded Thumbnail',
      description: 'Large thumbnail above content',
      preview: (
        <div className="w-full bg-muted rounded-lg p-2 space-y-2">
          <div className="aspect-square bg-primary/20 rounded" />
          <div className="space-y-1">
            <div className="h-2 bg-foreground/20 rounded w-3/4" />
            <div className="h-1.5 bg-muted-foreground/20 rounded w-1/2" />
          </div>
        </div>
      ),
    },
  ];

  return (
    <div className={cn('space-y-3', className)}>
      <label className="text-sm font-medium text-foreground">Thumbnail Display</label>

      <div className="grid grid-cols-2 gap-3">
        {options.map((option) => (
          <button
            key={option.value}
            type="button"
            onClick={() => onChange(option.value)}
            className={cn(
              'relative p-3 rounded-lg border-2 transition-all text-left',
              'hover:border-primary/50',
              value === option.value ? 'border-primary bg-primary/5' : 'border-border bg-card',
            )}
          >
            {/* Preview */}
            <div className="aspect-[4/3] mb-3">{option.preview}</div>

            {/* Label and description */}
            <div className="space-y-1">
              <div className={cn('text-sm font-medium', value === option.value ? 'text-primary' : 'text-foreground')}>{option.label}</div>
              <div className="text-xs text-muted-foreground">{option.description}</div>
            </div>

            {/* Selection indicator */}
            {value === option.value && (
              <div className="absolute top-2 right-2 w-4 h-4 bg-primary rounded-full flex items-center justify-center">
                <div className="w-2 h-2 bg-primary-foreground rounded-full" />
              </div>
            )}
          </button>
        ))}
      </div>
    </div>
  );
}
