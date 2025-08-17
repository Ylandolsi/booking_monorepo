import ReactDOM from 'react-dom/client';
import './index.css';
import { AppProvider } from './providers/app-provider';

ReactDOM.createRoot(document.getElementById('root')!).render(
  // <React.StrictMode>
  <AppProvider />,
  // {/* </React.StrictMode>, */}
);
