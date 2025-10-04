import { useState } from 'react';
import { toast } from 'sonner';

export const useCopyToClipboard = () => {
  const [content, setContent] = useState<string>('');
  const handleCopy = async (link: string) => {
    try {
      await navigator.clipboard.writeText(link);
      toast.success('Copied to clipboard !');
      setContent(link);
    } catch (e) {
      // Fallback for older browsers
      const textArea = document.createElement('textarea');
      textArea.value = link;
      document.body.appendChild(textArea);
      textArea.select();
      document.execCommand('copy');
      document.body.removeChild(textArea);
      setContent(link);
      // setTimeout(() => setCopied(false), 2000);
    }
  };
  return { content, handleCopy };
};
