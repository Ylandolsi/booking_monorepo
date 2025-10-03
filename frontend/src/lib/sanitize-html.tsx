import DOMPurify from 'dompurify';

export function SanitizeHtml({ htmlContent }: { htmlContent: string }) {
  const cleanHTML = DOMPurify.sanitize(htmlContent);
  return <div dangerouslySetInnerHTML={{ __html: cleanHTML }} />;
}
