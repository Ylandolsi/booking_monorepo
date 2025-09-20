import { cn } from '@/lib/cn';

type DisplayMode = 'full' | 'compact';

interface DisplayModeSelectorProps {
  displayMode: DisplayMode;
  onChange: (mode: DisplayMode) => void;
  className?: string;
}

export function DisplayModeSelector({ displayMode, onChange, className }: DisplayModeSelectorProps) {
  return (
    <div className={cn('flex items-center space-x-2', className)}>
      <span className="text-sm font-medium text-foreground">Display:</span>

      <div className="flex bg-muted rounded-lg p-1">
        <button
          onClick={() => onChange('full')}
          className={cn(
            'px-3 py-1 text-xs font-medium rounded-md transition-colors',
            displayMode === 'full' ? 'bg-primary text-primary-foreground' : 'text-muted-foreground hover:text-foreground',
          )}
        >
          ⬜ Full
        </button>

        <button
          onClick={() => onChange('compact')}
          className={cn(
            'px-3 py-1 text-xs font-medium rounded-md transition-colors',
            displayMode === 'compact' ? 'bg-primary text-primary-foreground' : 'text-muted-foreground hover:text-foreground',
          )}
        >
          ☰ Compact
        </button>
      </div>
    </div>
  );
}
