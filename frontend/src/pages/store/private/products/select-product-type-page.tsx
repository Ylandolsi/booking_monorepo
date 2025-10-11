import type { ProductType } from '@/api/stores';
import { Button } from '@/components';
import { routes } from '@/config';
import { useAppNavigation } from '@/hooks';
import { cn } from '@/lib';

/// TODO : fetch this from the backend
const productTypes = [
  {
    id: 'Session',
    title: '1:1 Call Booking',
    description: 'Schedule consultations, coaching calls, or meetings',
    icon: '📅',
    color: 'bg-blue-50 border-blue-200 dark:bg-blue-950/20 dark:border-blue-800',
  },
  {
    id: 'DigitalDownload',
    title: 'Digital Download',
    description: 'Sell ebooks, courses, templates, or digital files',
    icon: '📁',
    color: 'bg-purple-50 border-purple-200 dark:bg-purple-950/20 dark:border-purple-800',
  },
] as const;

interface SelectProductTypeProps {
  className?: string;
}
export const SelectProductType = ({ className }: SelectProductTypeProps) => {
  const navigate = useAppNavigation();

  const onSelect = (type: ProductType) => {
    navigate.goTo({ to: routes.to.store.productAdd({ type }), replace: true });
  };

  const onCancel = () => {
    const indexStore = routes.to.store.index().endsWith('/') ? routes.to.store.index().slice(0, -1) : routes.to.store.index();
    navigate.goTo({ to: indexStore, replace: true });
  };

  return (
    <div className="bg-card border-border rounded-xl border p-6 shadow-lg">
      <div className={cn('space-y-4', className)}>
        <div className="mb-6 text-center">
          <h2 className="text-foreground mb-2 text-xl font-semibold">What type of product are you creating?</h2>
          <p className="text-accent-foreground">Choose the type that best fits what you want to sell</p>
        </div>

        <div className="flex flex-col gap-2 md:flex-row">
          {productTypes.map((type) => (
            <button
              key={type.id}
              onClick={() => onSelect(type.id)}
              className={cn('w-full rounded-xl border-2 p-6 text-left transition-all', 'hover:shadow-md', type.color)}
            >
              <div className="flex items-start space-x-4">
                <div className="text-3xl">{type.icon}</div>
                <div className="flex-1">
                  <h3 className="text-foreground mb-1 font-semibold">{type.title}</h3>
                  <p className="text-accent-foreground text-sm leading-relaxed">{type.description}</p>
                </div>
                <div className="text-accent-foreground">→</div>
              </div>
            </button>
          ))}
        </div>
      </div>
      {/* Cancel Button */}
      <div className="mt-6 text-center">
        <Button variant={'outline'} onClick={onCancel} className="text-accent-foreground hover:text-foreground text-sm transition-colors">
          Cancel
        </Button>
      </div>
    </div>
  );
};
