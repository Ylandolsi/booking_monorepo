import { cn } from '@/lib/cn';

interface EmptyStateProps {
  onAddProduct?: () => void;
  className?: string;
}

export function EmptyState({ onAddProduct, className }: EmptyStateProps) {
  return (
    <div className={cn('px-6 py-12 text-center', className)}>
      {/* Illustration */}
      <div className="mb-6">
        <div className="bg-accent mx-auto flex h-24 w-24 items-center justify-center rounded-full">
          <span className="text-4xl">📦</span>
        </div>
      </div>

      {/* Content */}
      <h3 className="text-foreground mb-2 text-lg font-semibold">Your store is empty</h3>

      <p className="text-muted-foreground mb-6 leading-relaxed">
        Add your first product to start selling. You can create bookings for 1:1 calls or sell digital downloads.
      </p>

      {/* CTA Button */}
      {onAddProduct && (
        <button
          onClick={onAddProduct}
          className="bg-primary text-primary-foreground rounded-lg px-6 py-3 font-semibold transition-opacity hover:opacity-90"
        >
          Add Your First Product
        </button>
      )}
    </div>
  );
}
