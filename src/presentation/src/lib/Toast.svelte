<script lang="ts">
  import { onMount } from "svelte";
  import { fade, fly } from "svelte/transition";

  export let type: "success" | "error" = "success";
  export let message: string;
  export let duration: number = 3000;
  export let onClose: () => void = () => {};

  onMount(() => {
    const timer = setTimeout(() => {
      onClose();
    }, duration);

    return () => clearTimeout(timer);
  });
</script>

<div
  class={`fixed top-4 right-4 z-50 flex items-center gap-3 px-6 py-4 rounded-lg shadow-lg border animate-in fade-in slide-in-from-top-4 duration-300 ${
    type === "success"
      ? "bg-emerald-900/90 border-emerald-500/50"
      : "bg-red-900/90 border-red-500/50"
  }`}
>
  {#if type === "success"}
    <div
      class="w-8 h-8 rounded-full bg-emerald-500/20 flex items-center justify-center"
    >
      <svg
        class="w-5 h-5 text-emerald-500"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M5 13l4 4L19 7"
        />
      </svg>
    </div>
  {:else}
    <div
      class="w-8 h-8 rounded-full bg-red-500/20 flex items-center justify-center"
    >
      <svg
        class="w-5 h-5 text-red-500"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M6 18L18 6M6 6l12 12"
        />
      </svg>
    </div>
  {/if}
  <p class="text-white text-base font-medium">{message}</p>
</div>
