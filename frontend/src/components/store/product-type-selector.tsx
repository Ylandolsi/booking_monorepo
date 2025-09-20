import { cn } from '@/lib/cn';

const productTypes = [
  {
    id: 'booking',
    title: '1:1 Call Booking',
    description: 'Schedule consultations, coaching calls, or meetings',
    icon: '📅',
    color: 'bg-blue-50 border-blue-200 dark:bg-blue-950/20 dark:border-blue-800',
  },
  {
    id: 'digital',
    title: 'Digital Download',
    description: 'Sell ebooks, courses, templates, or digital files',
    icon: '📁',
    color: 'bg-purple-50 border-purple-200 dark:bg-purple-950/20 dark:border-purple-800',
  },
] as const;

interface ProductTypeSelectorProps {
  onSelect: (type: 'booking' | 'digital') => void;
  className?: string;
}

export function ProductTypeSelector({ onSelect, className }: ProductTypeSelectorProps) {
  return (
    <div className={cn('space-y-4', className)}>
      <div className="text-center mb-6">
        <h2 className="text-xl font-semibold text-foreground mb-2">What type of product are you creating?</h2>
        <p className="text-muted-foreground">Choose the type that best fits what you want to sell</p>
      </div>

      <div className="space-y-3">
        {productTypes.map((type) => (
          <button
            key={type.id}
            onClick={() => onSelect(type.id)}
            className={cn('w-full p-6 rounded-xl border-2 text-left transition-all', 'hover:border-primary hover:shadow-md', type.color)}
          >
            <div className="flex items-start space-x-4">
              <div className="text-3xl">{type.icon}</div>
              <div className="flex-1">
                <h3 className="font-semibold text-foreground mb-1">{type.title}</h3>
                <p className="text-sm text-muted-foreground leading-relaxed">{type.description}</p>
              </div>
              <div className="text-muted-foreground">→</div>
            </div>
          </button>
        ))}
      </div>
    </div>
  );
}
