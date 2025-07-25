// change the title and meta of html page => improve the SEO 
import { Helmet } from 'react-helmet-async';
type HeadProps = {
  title?: string;
  description?: string;
};

export const Head = ({ title = '', description = '' }: HeadProps = {}) => {
  return (
    <Helmet>
      <title>{title ? `${title} | Booking app` : 'Booking app'}</title>
      <meta name="description" content={description} />
    </Helmet>
  );
};
