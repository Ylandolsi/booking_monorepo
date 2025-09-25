import { MobileContainer } from '@/components';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/store/checkout')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <MobileContainer>
      <div className="bg-background-light dark:bg-background-dark relative min-h-screen text-slate-800 dark:text-slate-200">
        <header className="absolute top-0 right-0 left-0 z-10 flex items-center justify-between p-4">
          <a className="flex items-center gap-2" href="#">
            <span className="bg-primary/20 dark:bg-primary/30 flex h-6 w-6 items-center justify-center rounded-full">
              <svg className="text-primary h-4 w-4" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
                <path
                  clip-rule="evenodd"
                  d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z"
                  fill-rule="evenodd"
                ></path>
              </svg>
            </span>
            <span className="font-bold text-slate-900 dark:text-white">My Store</span>
          </a>
        </header>
        <main className="pb-28">
          <div className="h-80 w-full">
            <img
              alt="Product Image"
              className="h-full w-full object-cover"
              src="https://lh3.googleusercontent.com/aida-public/AB6AXuA7k7Y1yTUsoGw4kHQ-au2cb85NtWAxnzb8R4sAvMlM73WrWUrUhuzqa4i7fvFJkbZWWSamtgVPoY-LtYj2gEnfzCm_dM2sMbL8X7I9DyaGB-5lL6f2r-p6sHYczA-ID-kqquaLSMCTtOhr2ykI8_fET6STytxU4CjxNKoOZN_VDA-lcAdOBxwmj8rzTr0mciZU8q_VMQfWb5WYSZO7zwbSrPShrVxxD30-i6ZZsBXgQjO5wAHROE-LgHbSv_-CIKLF4K2o7rbJB-Zs"
            />
          </div>
          <div className="space-y-5 p-6">
            <h1 className="text-3xl font-extrabold text-slate-900 dark:text-white">The Ultimate Guide to Design</h1>
            <p className="mt-2 text-lg text-slate-600 dark:text-slate-400">Master the art of modern UI/UX</p>
            <div className="mt-4">
              <span className="text-primary text-4xl font-bold">$49</span>
            </div>
            <div className="mt-6 space-y-4 text-slate-700 dark:text-slate-300">
              <h2 className="text-xl font-bold text-slate-900 dark:text-white">Description</h2>
              <p>
                This is a detailed description of the product, highlighting its features, benefits, and any other relevant information that would help
                a customer make a purchase decision. It's designed to be comprehensive yet easy to read.
              </p>
              <p>Dive deep into principles of visual hierarchy, color theory, and interaction design.</p>
            </div>
            <div className="bg-primary/10 dark:bg-primary/20 mt-6 rounded-lg p-4">
              <h3 className="font-bold text-slate-900 dark:text-white">Fulfillment Note</h3>
              <p className="mt-1 text-sm text-slate-700 dark:text-slate-300">
                A download link for the digital product will be sent to your email address immediately after purchase.
              </p>
            </div>
            <button className="bg-primary shadow-primary/30 hover:bg-opacity-90 h-14 w-full rounded-xl text-lg font-bold text-white shadow-lg transition-all duration-300">
              Buy Now
            </button>
          </div>
        </main>
      </div>
    </MobileContainer>
  );
}
