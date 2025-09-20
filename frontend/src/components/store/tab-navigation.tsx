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
    <div className={cn('border-b border-border bg-card', className)}>
      <div className="flex space-x-1 p-1">
        {tabs.map((tab) => (
          <button
            key={tab.id}
            onClick={() => onTabChange(tab.id)}
            className={cn(
              'flex-1 px-4 py-3 rounded-lg text-sm font-medium transition-all',
              'hover:bg-muted/50 focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2',
              activeTab === tab.id ? 'bg-primary text-primary-foreground shadow-sm' : 'text-muted-foreground hover:text-foreground',
            )}
          >
            <div className="flex items-center justify-center space-x-2">
              {tab.icon && <span className="text-lg">{tab.icon}</span>}
              <div>
                <div className="font-semibold">{tab.label}</div>
                {tab.description && (
                  <div className={cn('text-xs mt-0.5', activeTab === tab.id ? 'text-primary-foreground/80' : 'text-muted-foreground')}>
                    {tab.description}
                  </div>
                )}
              </div>
            </div>
          </button>
        ))}
      </div>
    </div>
  );
}
