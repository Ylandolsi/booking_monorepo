import { Input, Label } from '@/components/ui';
import { useCopyToClipboard } from '@/hooks';
import { Copy } from 'lucide-react';

export const InputToCopy = ({ input, className, label }: { input: string; className?: string; label: string }) => {
  const { content, handleCopy } = useCopyToClipboard();
  return (
    <div className={`${className}`}>
      <Label className="block font-semibold">
        {label}
        <div className="relative mt-2">
          <Input type="text" value={input} />
          <button
            type="button"
            onClick={() => handleCopy(input || '')}
            aria-label="Copy meeting link"
            className="absolute top-1/2 right-3 -translate-y-1/2 transform hover:cursor-pointer"
          >
            <Copy size={20} />
          </button>
          {content && <span className="text-primary absolute top-2 right-12 text-sm">Copied!</span>}
        </div>
      </Label>
    </div>
  );
};
