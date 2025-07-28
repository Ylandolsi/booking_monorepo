import { create } from 'zustand';

type GlobalErrorState = {
  error: Error | null;
  setError: (error: Error) => void;
  clearError: () => void;
};

export const useGlobalErrorStore = create<GlobalErrorState>((set) => ({
  error: null,
  setError: (error: Error) => set({ error }),
  clearError: () => set({ error: null }),
}));
