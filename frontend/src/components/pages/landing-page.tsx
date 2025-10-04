import { Star } from 'lucide-react';
import { Logo } from '@/components/logo';
import { Button } from '@/components/ui';
import { useAppNavigation } from '@/hooks';
import { useState } from 'react';
import { ArrowRight, Menu, X } from 'lucide-react';
import { motion, AnimatePresence } from 'motion/react';
import { ContainerScroll } from '@/components/ui/container-scroll-animation';

// export const LandingPage = () => {
//   const navigate = useAppNavigation();
//   return (
//     <div
//       className="min-h-screen bg-white"
//       style={{
//         fontFamily: 'Inter, sans-serif',
//         color: 'oklch(0.3211 0 0)',
//       }}
//     >
//       <section className="relative flex h-screen items-center justify-center overflow-hidden px-4 pt-20 pb-16 sm:px-6 lg:px-8">
//         <div className="mx-auto max-w-7xl">
//           <div className="relative z-10 text-center">
//             {/* Social proof badge */}
//             <div
//               className="mb-8 inline-flex items-center gap-2 rounded-full border px-4 py-2"
//               style={{
//                 backgroundColor: 'oklch(0.9846 0.0017 247.8389)',
//                 borderColor: 'oklch(0.9276 0.0058 264.5313)',
//               }}
//             >
//               <div className="flex items-center">
//                 <Logo />
//               </div>
//               <Star className="h-4 w-4" style={{ color: 'oklch(0.2781 0.0296 256.848)' }} />
//               <span className="text-sm font-medium">Trusted by +100 mentors in Tunisia </span>
//             </div>

//             <h1 className="mb-6 text-4xl font-bold tracking-tight md:text-4xl">
//               Schedule meetings that
//               <br />
//               <span className="from-primary to-chart-3 bg-gradient-to-r bg-clip-text text-transparent">actually happen</span>
//             </h1>

//             <p className="mx-auto mb-10 max-w-2xl text-lg" style={{ color: 'oklch(0.551 0.0234 264.3637)' }}>
//               Stop the back-and-forth emails. BookFlow automatically finds the perfect time for everyone and sends smart reminders that eliminate
//               no-shows.
//             </p>

//             <div className="mx-auto max-w-4xl text-center">
//               {/* <h2 className="text-3xl md:text-4xl font-semibold mb-6">Ready to eliminate scheduling chaos?</h2> */}

//               <div className="flex flex-col justify-center gap-4 sm:flex-row">
//                 <Button
//                   className="h-10 transform rounded-full p-4 px-8 text-lg font-semibold text-white shadow-lg transition-all hover:scale-105"
//                   style={{ backgroundColor: 'oklch(0.2781 0.0296 256.848)' }}
//                   onClick={() => navigate.goToApp()}
//                 >
//                   Start your journey
//                   <ArrowRight className="ml-2 inline h-5 w-5" />
//                 </Button>
//                 <Button
//                   variant={'ghost'}
//                   className="h-10 rounded-full border-2 p-4 px-8 text-lg font-semibold shadow-lg transition-colors hover:scale-105"
//                 >
//                   Schedule a Demo
//                 </Button>
//               </div>

//               <p className="mt-6 text-sm"> ✓ Setup in under 60 seconds ✓ Cancel anytime</p>
//             </div>
//           </div>
//         </div>

//         {/* Background decoration */}
//         <div className="absolute inset-0 -z-10 overflow-hidden">
//           <div
//             className="absolute -top-40 -right-40 h-80 w-80 rounded-full opacity-20 blur-3xl"
//             style={{ backgroundColor: 'oklch(0.2781 0.0296 256.848)' }}
//           />
//           <div
//             className="absolute -bottom-40 -left-40 h-80 w-80 rounded-full opacity-20 blur-3xl"
//             style={{ backgroundColor: 'oklch(0.7183 0.0812 257.477)' }}
//           />
//         </div>
//       </section>
//     </div>
//   );
// };

export const LandingPage = () => {
  return (
    <div className="relative min-h-screen w-full bg-[#fefcff]">
      {/* Dreamy Sky Pink Glow */}
      <div
        className="absolute inset-0 z-0"
        style={{
          backgroundImage: `
      radial-gradient(circle at 30% 70%, rgba(173, 216, 230, 0.35), transparent 60%),
      radial-gradient(circle at 70% 30%, rgba(255, 182, 193, 0.4), transparent 60%)`,
        }}
      />
      <Hero2 />
      {/* Your Content/Components */}
    </div>
  );
};

const Hero2 = () => {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const navigate = useAppNavigation();

  return (
    <div className="relative min-h-screen overflow-hidden bg-black">
      {/* Gradient background with grain effect */}
      <div className="absolute -top-10 -right-60 z-0 flex flex-col items-end blur-xl">
        <div className="from-primary z-1 h-[10rem] w-[60rem] rounded-full bg-gradient-to-b to-sky-600 blur-[6rem]"></div>
        <div className="from-primary z-1 h-[10rem] w-[90rem] rounded-full bg-gradient-to-b to-yellow-400 blur-[6rem]"></div>
        <div className="from-primary z-1 h-[10rem] w-[60rem] rounded-full bg-gradient-to-b to-sky-500 blur-[6rem]"></div>
      </div>
      <div className="bg-noise absolute inset-0 z-0 opacity-30"></div>

      {/* Content container */}
      <div className="relative z-10">
        {/* Navigation */}
        <nav className="container mx-auto mt-6 flex items-center justify-between px-4 py-4">
          <div className="flex items-center">
            <div className="flex h-8 w-8 items-center justify-center rounded-full bg-white text-black">
              <span className="font-bold">⚡</span>
            </div>
            <span className="ml-2 text-xl font-bold text-white">LeadGenie</span>
          </div>

          {/* Desktop Navigation */}
          <div className="hidden items-center space-x-6 md:flex">
            <div className="flex items-center space-x-6">
              <NavItem label="Use Cases" hasDropdown />
              <NavItem label="Products" hasDropdown />
              <NavItem label="Resources" hasDropdown />
              <NavItem label="Pricing" />
            </div>
            <div className="flex items-center space-x-3">
              <button
                className="h-12 rounded-full bg-white px-8 text-base font-medium text-black hover:bg-white/90"
                onClick={() => navigate.goToLogin()}
              >
                Login
              </button>
            </div>
          </div>

          {/* Mobile menu button */}
          <button className="md:hidden" onClick={() => setMobileMenuOpen(!mobileMenuOpen)}>
            <span className="sr-only">Toggle menu</span>
            {mobileMenuOpen ? <X className="h-6 w-6 text-white" /> : <Menu className="h-6 w-6 text-white" />}
          </button>
        </nav>

        {/* Mobile Navigation Menu with animation */}
        <AnimatePresence>
          {mobileMenuOpen && (
            <motion.div
              initial={{ y: '-100%' }}
              animate={{ y: 0 }}
              exit={{ y: '-100%' }}
              transition={{ duration: 0.3 }}
              className="fixed inset-0 z-50 flex flex-col bg-black/95 p-4 md:hidden"
            >
              <div className="flex items-center justify-between">
                <div className="flex items-center">
                  <div className="flex h-8 w-8 items-center justify-center rounded-full bg-white text-black">
                    <span className="font-bold">⚡</span>
                  </div>
                  <span className="ml-2 text-xl font-bold text-white">LeadGenie</span>
                </div>
                <button onClick={() => setMobileMenuOpen(false)}>
                  <X className="h-6 w-6 text-white" />
                </button>
              </div>
              <div className="mt-8 flex flex-col space-y-6">
                <MobileNavItem label="Use Cases" />
                <MobileNavItem label="Products" />
                <MobileNavItem label="Resources" />
                <MobileNavItem label="Pricing" />
                <div className="pt-4">
                  <button className="w-full justify-start border border-gray-700 text-white">Log in</button>
                </div>
                <button className="h-12 rounded-full bg-white px-8 text-base font-medium text-black hover:bg-white/90">Get Started For Free</button>
              </div>
            </motion.div>
          )}
        </AnimatePresence>

        {/* Badge */}
        <div className="mx-auto mt-6 flex max-w-fit items-center justify-center space-x-2 rounded-full bg-white/10 px-4 py-2 backdrop-blur-sm">
          <span className="text-sm font-medium text-white">Join the revolution today!</span>
          <ArrowRight className="h-4 w-4 text-white" />
        </div>

        {/* Hero section */}
        <div className="container mx-auto mt-12 px-4 text-center">
          <h1 className="mx-auto max-w-6xl text-5xl leading-tight font-bold text-white md:text-6xl lg:text-7xl">
            Store in minutes
            <br /> Connnect with your audience
          </h1>
          <p className="mx-auto mt-6 max-w-2xl text-lg text-gray-300">
            Delivering unmatched email campaigns every day at unbeatable rates. Our tool redefines cost-effectiveness. Now!!!
          </p>
          <div className="mt-10 flex flex-col items-center justify-center space-y-4 sm:flex-row sm:space-y-0 sm:space-x-4">
            <button className="h-12 rounded-full bg-white px-8 text-base font-medium text-black hover:bg-white/90">
              Start Your 7 Day Free Trial
            </button>
            <button className="h-12 rounded-full border border-gray-600 px-8 text-base font-medium text-white hover:bg-white/10">Watch Demo</button>
          </div>

          <div className="-mt-50">
            <ContainerScroll titleComponent={'test'}>
              {/* <div className="bg-grainy absolute inset-0 rounded bg-white opacity-20 shadow-lg blur-[10rem]" /> */}

              {/* Hero Image */}
              <img
                src="https://kikxai.netlify.app/_next/image?url=%2Fassets%2Fhero-image.png&w=1920&q=75"
                alt="Hero Image"
                className="relative h-auto w-full rounded shadow-md grayscale-100"
              />
            </ContainerScroll>
          </div>
        </div>
      </div>
    </div>
  );
};

function NavItem({ label, hasDropdown }: { label: string; hasDropdown?: boolean }) {
  return (
    <div className="flex items-center text-sm text-gray-300 hover:text-white">
      <span>{label}</span>
      {hasDropdown && (
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="16"
          height="16"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          strokeWidth="2"
          strokeLinecap="round"
          strokeLinejoin="round"
          className="ml-1"
        >
          <path d="m6 9 6 6 6-6" />
        </svg>
      )}
    </div>
  );
}

function MobileNavItem({ label }: { label: string }) {
  return (
    <div className="flex items-center justify-between border-b border-gray-800 pb-2 text-lg text-white">
      <span>{label}</span>
      <ArrowRight className="h-4 w-4 text-gray-400" />
    </div>
  );
}

export { Hero2 };

export const DreamySkyPinkGlow = ({}) => {
  return (
    <div className="relative min-h-screen w-full bg-[#fefcff]">
      {/* Dreamy Sky Pink Glow */}
      <div
        className="absolute inset-0 z-0"
        style={{
          backgroundImage: `
      radial-gradient(circle at 30% 70%, rgba(173, 216, 230, 0.35), transparent 60%),
      radial-gradient(circle at 70% 30%, rgba(255, 182, 193, 0.4), transparent 60%)`,
        }}
      />
      {/* Your Content/Components */}
    </div>
  );
};
