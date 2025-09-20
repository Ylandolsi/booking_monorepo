import { cn } from '@/lib/cn';

interface AddProductButtonProps {
  onClick?: () => void;
  className?: string;
}

export function AddProductButton({ onClick, className }: AddProductButtonProps) {
  return (
    <button
      onClick={onClick}
      className={cn(
        'w-full py-4 border-2 border-dashed border-border rounded-xl',
        'text-muted-foreground hover:text-foreground hover:border-foreground/30',
        'transition-colors duration-200',
        'flex items-center justify-center space-x-2',
        className,
      )}
    >
      <span className="text-xl">+</span>
      <span className="font-medium">Add Product</span>
    </button>
  );
}
