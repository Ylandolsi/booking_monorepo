import type { Item } from '@/components/navigation/side-bar/sidebar';
import { Button } from '@/components/ui';
import { useAppNavigation } from '@/hooks';
import { FaMoneyCheck } from 'react-icons/fa';

export function AdminSection(props: { handleItemClick: (item: Item) => void; itemActive: string; collapsed: boolean }) {
  const nav = useAppNavigation();
  const adminItems: Item[] = [
    {
      name: 'Payouts Requests',
      icon: <FaMoneyCheck size={20} />,
      click: () => nav.goToAdminPayoutRequests(),
    },
  ];

  return (
    <div className="mb-6">
      {!props.collapsed && <h3 className="text-accent-foreground mb-3 px-2 text-xs font-semibold tracking-wider uppercase">Admin</h3>}
      <nav className="space-y-1">
        {adminItems.map(function (item) {
          return (
            <Button
              key={item.name}
              onClick={() => props.handleItemClick(item)}
              title={props.collapsed ? item.name : undefined}
              variant="ghost"
              className={`relative flex w-full items-center rounded-xl text-left transition-all duration-200 ${props.collapsed ? 'justify-center p-2' : 'gap-3 px-3 py-2.5'} ${props.itemActive === item.name ? 'bg-primary text-primary-foreground shadow-md' : 'hover:bg-accent text-accent-foreground hover:text-foreground'} }`}
            >
              <span>{item.icon}</span>
              {!props.collapsed && (
                <>
                  <span className="flex-1 font-medium">{item.name}</span>
                </>
              )}
            </Button>
          );
        })}
      </nav>
    </div>
  );
}
