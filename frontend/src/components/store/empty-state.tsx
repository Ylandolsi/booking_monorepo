import { cn } from '@/lib/cn';

interface EmptyStateProps {
  onAddProduct?: () => void;
  className?: string;
}

export function EmptyState({ onAddProduct, className }: EmptyStateProps) {
  return (
    <div className={cn('text-center py-12 px-6', className)}>
      {/* Illustration */}
      <div className="mb-6">
        <div className="w-24 h-24 mx-auto bg-muted rounded-full flex items-center justify-center">
          <span className="text-4xl">📦</span>
        </div>
      </div>

      {/* Content */}
      <h3 className="text-lg font-semibold text-foreground mb-2">Your store is empty</h3>

      <p className="text-muted-foreground mb-6 leading-relaxed">
        Add your first product to start selling. You can create bookings for 1:1 calls or sell digital downloads.
      </p>

      {/* CTA Button */}
      {onAddProduct && (
        <button
          onClick={onAddProduct}
          className="bg-primary text-primary-foreground px-6 py-3 rounded-lg font-semibold hover:opacity-90 transition-opacity"
        >
          Add Your First Product
        </button>
      )}
    </div>
  );
}
