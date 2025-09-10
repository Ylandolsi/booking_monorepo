import { Star, ArrowRight } from 'lucide-react';
import { Logo } from '@/components/logo';

export const LandingPage = () => {
  return (
    <div
      className="min-h-screen bg-white"
      style={{
        fontFamily: 'Inter, sans-serif',
        color: 'oklch(0.3211 0 0)',
      }}
    >
      {/* Hero Section */}
      <section className="h-screen flex items-center justify-center pt-20 pb-16 px-4 sm:px-6 lg:px-8 relative overflow-hidden">
        <div className="max-w-7xl mx-auto">
          <div className="text-center relative z-10">
            {/* Social proof badge */}
            <div
              className="inline-flex items-center gap-2 px-4 py-2 rounded-full mb-8 border"
              style={{
                backgroundColor: 'oklch(0.9846 0.0017 247.8389)',
                borderColor: 'oklch(0.9276 0.0058 264.5313)',
              }}
            >
              <div className="flex items-center">
                <Logo />
              </div>
              <Star className="h-4 w-4" style={{ color: 'oklch(0.2781 0.0296 256.848)' }} />
              <span className="text-sm font-medium">Trusted by +100 mentors in Tunisia </span>
            </div>

            <h1 className="text-4xl md:text-6xl lg:text-7xl font-bold tracking-tight mb-6">
              Schedule meetings that
              <br />
              <span className="bg-gradient-to-r from-primary to-chart-3 bg-clip-text text-transparent">actually happen</span>
            </h1>

            <p className="max-w-2xl mx-auto text-lg md:text-xl mb-10" style={{ color: 'oklch(0.551 0.0234 264.3637)' }}>
              Stop the back-and-forth emails. BookFlow automatically finds the perfect time for everyone and sends smart reminders that eliminate
              no-shows.
            </p>

            <div className="max-w-4xl mx-auto text-center">
              {/* <h2 className="text-3xl md:text-4xl font-semibold mb-6">Ready to eliminate scheduling chaos?</h2> */}

              <div className="flex flex-col sm:flex-row gap-4 justify-center">
                <button
                  className="px-8 py-4 rounded-full text-white font-semibold text-lg transition-all transform hover:scale-105 shadow-lg"
                  style={{ backgroundColor: 'oklch(0.2781 0.0296 256.848)' }}
                >
                  Start your journey
                  <ArrowRight className="ml-2 h-5 w-5 inline" />
                </button>
                <button
                  className="px-8 py-4 rounded-full font-semibold text-lg border-2 transition-colors"
                  style={{
                    borderColor: 'oklch(0.9276 0.0058 264.5313)',
                    color: 'oklch(0.3211 0 0)',
                  }}
                >
                  Schedule a Demo
                </button>
              </div>

              <p className="mt-6 text-sm" style={{ color: 'oklch(0.551 0.0234 264.3637)' }}>
                ✓ No credit card required ✓ Setup in under 60 seconds ✓ Cancel anytime
              </p>
            </div>
          </div>
        </div>

        {/* Background decoration */}
        <div className="absolute inset-0 -z-10 overflow-hidden">
          <div
            className="absolute -top-40 -right-40 w-80 h-80 rounded-full opacity-20 blur-3xl"
            style={{ backgroundColor: 'oklch(0.2781 0.0296 256.848)' }}
          />
          <div
            className="absolute -bottom-40 -left-40 w-80 h-80 rounded-full opacity-20 blur-3xl"
            style={{ backgroundColor: 'oklch(0.7183 0.0812 257.477)' }}
          />
        </div>
      </section>
    </div>
  );
};
