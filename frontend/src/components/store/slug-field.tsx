import { useState } from 'react';
import { cn } from '@/lib/cn';

interface SlugFieldProps {
  value: string;
  onChange: (value: string) => void;
  isAvailable?: boolean;
  baseUrl?: string;
  className?: string;
}

export function SlugField({ value, onChange, isAvailable, baseUrl = 'mystore.com', className }: SlugFieldProps) {
  const [isFocused, setIsFocused] = useState(false);

  const getValidationColor = () => {
    if (!value || value.length < 3) return '';
    return isAvailable ? 'border-green-500' : 'border-destructive';
  };

  const getValidationIcon = () => {
    if (!value || value.length < 3) return null;
    return isAvailable ? <span className="text-green-500 text-sm">✓</span> : <span className="text-destructive text-sm">✗</span>;
  };

  return (
    <div className={cn('space-y-2', className)}>
      <label className="block text-sm font-medium text-foreground">Store URL</label>

      <div
        className={cn(
          'flex items-center bg-input border rounded-lg overflow-hidden transition-colors',
          isFocused ? 'ring-2 ring-ring' : '',
          getValidationColor(),
        )}
      >
        <span className="px-3 py-2 text-sm text-muted-foreground bg-muted border-r border-border">{baseUrl}/</span>
        <input
          type="text"
          value={value}
          onChange={(e) => onChange(e.target.value.toLowerCase().replace(/[^a-z0-9-]/g, ''))}
          onFocus={() => setIsFocused(true)}
          onBlur={() => setIsFocused(false)}
          placeholder="username"
          className="flex-1 px-3 py-2 bg-transparent text-foreground placeholder:text-muted-foreground focus:outline-none"
          minLength={3}
          maxLength={30}
        />
        <div className="px-3 py-2">{getValidationIcon()}</div>
      </div>

      {value && value.length >= 3 && (
        <p className={cn('text-xs', isAvailable ? 'text-green-600' : 'text-destructive')}>
          {isAvailable ? 'This username is available!' : 'This username is already taken'}
        </p>
      )}

      {value && value.length < 3 && <p className="text-xs text-muted-foreground">Username must be at least 3 characters</p>}
    </div>
  );
}
