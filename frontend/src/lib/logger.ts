import { env } from '@/config/env';

type LogLevel = 'debug' | 'info' | 'warn' | 'error';

class Logger {
  private isDevelopment = env.VITE_ENVIRONMENT === 'development';

  private log(level: LogLevel, message: string, ...args: any[]) {
    // Only log in development or for errors in production
    if (this.isDevelopment || level === 'error') {
      const timestamp = new Date().toISOString();
      const prefix = `[${timestamp}] [${level.toUpperCase()}]`;

      switch (level) {
        case 'debug':
          if (this.isDevelopment) console.debug(prefix, message, ...args);
          break;
        case 'info':
          if (this.isDevelopment) console.info(prefix, message, ...args);
          break;
        case 'warn':
          console.warn(prefix, message, ...args);
          break;
        case 'error':
          console.error(prefix, message, ...args);
          // TODO: Send to error tracking service (Sentry, LogRocket, etc.)
          this.sendToErrorTracking(message, args);
          break;
      }
    }
  }

  private sendToErrorTracking(message: string, args: any[]) {
    // Integrate with Sentry or similar service
    // if (window.Sentry) {
    //   window.Sentry.captureException(new Error(message), { extra: args });
    // }
  }

  debug(message: string, ...args: any[]) {
    this.log('debug', message, ...args);
  }

  info(message: string, ...args: any[]) {
    this.log('info', message, ...args);
  }

  warn(message: string, ...args: any[]) {
    this.log('warn', message, ...args);
  }

  error(message: string, ...args: any[]) {
    this.log('error', message, ...args);
  }
}

export const logger = new Logger();
