import { Button } from '@/components/ui';
import { cn } from '@/lib/cn';

interface TabOption {
  id: string;
  label: string;
  description?: string;
  icon?: string;
}

interface TabNavigationProps {
  tabs: TabOption[];
  activeTab: string;
  onTabChange: (tabId: string) => void;
  className?: string;
}

export function TabNavigation({ tabs, activeTab, onTabChange, className }: TabNavigationProps) {
  return (
    <div className={cn('bg-card', className)}>
      <div className="flex flex-wrap items-center justify-center gap-2 p-1">
        {tabs.map((tab) => (
          <Button
            key={tab.id}
            variant={'outline'}
            onClick={() => onTabChange(tab.id)}
            className={cn(
              'group flex-1 rounded-lg px-4 py-7 text-sm font-medium transition-all',
              'hover:bg-primary/20 hover:text-foreground focus:ring-ring focus:ring-2 focus:ring-offset-2 focus:outline-none',
              activeTab === tab.id ? 'bg-primary text-primary-foreground shadow-sm' : 'text-muted-foreground hover:text-foreground',
            )}
          >
            <div className="flex items-center justify-center space-x-2">
              {tab.icon && <span className="text-lg">{tab.icon}</span>}
              <div>
                <div className="font-semibold">{tab.label}</div>
                {tab.description && (
                  <div
                    className={cn(
                      'mt-0.5 text-xs',
                      activeTab === tab.id ? 'text-primary-foreground/80 group-hover:text-foreground' : 'text-muted-foreground',
                    )}
                  >
                    {tab.description}
                  </div>
                )}
              </div>
            </div>
          </Button>
        ))}
      </div>
    </div>
  );
}
