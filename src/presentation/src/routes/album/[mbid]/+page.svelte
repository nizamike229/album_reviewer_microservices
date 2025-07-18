<script lang="ts">
  import { page } from "$app/stores";
  import { onMount } from "svelte";
  import axios from "axios";
  import StarRating from "$lib/StarRating.svelte";
  import Toast from "$lib/Toast.svelte";
  import { browser } from "$app/environment";
  import { goto } from "$app/navigation";
  let apiClient: any;

  let isReviewModalOpen = false;
  let toast: { type: 'success' | 'error', message: string } | null = null;
  let reviewText = "";
  let userTrackRatings: number[] = [];
  let userAlbumRating = 0;
  let isSubmitting = false;
  let reviewError = "";

  function openReviewModal() {
    isReviewModalOpen = true;
    userTrackRatings = tracks.map(() => 0);
    userAlbumRating = 0;
    reviewText = "";
  }

  async function submitReview() {
    if (reviewText.length < 300) {
      reviewError = "Review must be at least 300 characters long";
      return;
    }
    if (userAlbumRating === 0) {
      reviewError = "Please rate the album";
      return;
    }
    if (userTrackRatings.some(r => r === 0)) {
      reviewError = "Please rate all tracks";
      return;
    }

    isSubmitting = true;
    reviewError = "";

    const trackRatingsObj = Object.fromEntries(
      userTrackRatings.map((rating, index) => [index + 1, rating])
    );

    try {
      const response = await apiClient.post("/api/review/publish", {
        rating: userAlbumRating,
        trackRatings: JSON.stringify(trackRatingsObj),
        description: reviewText,
        mbId: mbid
      });
      
      if (response.status === 200) {
        isReviewModalOpen = false;
        toast = {
          type: 'success',
          message: 'Review was published successfully!'
        };
        setTimeout(() => {
          goto("/");
        }, 2000);
      }
    } catch (e) {
      toast = {
        type: 'error',
        message: 'Error publishing review. Please try again later.'
      };
      reviewError = "Error publishing review. Please try again later.";
    } finally {
      isSubmitting = false;
    }
  }

  let mbid = "";
  $: mbid = $page.params.mbid;

  let albumTitle = "";
  let artist = "";
  let cover = "";
  let tracks: { title: string; length: number }[] = [];
  let albumRating = 0;
  let trackRatings: number[] = [];
  let loading = true;
  let error = "";

  function formatTime(ms: number): string {
    if (!ms) return "--:--";
    const minutes = Math.floor(ms / 60000);
    const seconds = Math.floor((ms % 60000) / 1000);
    return `${minutes}:${seconds.toString().padStart(2, "0")}`;
  }

  function formatRating(r: number): string {
    return r.toFixed(1);
  }

  onMount(async () => {
    if (browser) {
      const module = await import('$lib/apiClient');
      apiClient = module.default;
    }
    loading = true;
    try {
      const groupRes = await axios.get(
        `https://musicbrainz.org/ws/2/release-group/${mbid}`,
        {
          params: { fmt: "json", inc: "artist-credits" },
          headers: { "User-Agent": "YourApp/1.0 (youremail@example.com)" },
        }
      );
      albumTitle = groupRes.data.title;
      artist =
        groupRes.data["artist-credit"]?.[0]?.artist?.name ||
        groupRes.data["artist-credit"]?.[0]?.name ||
        "Unknown";
      cover = `https://coverartarchive.org/release-group/${mbid}/front`;

      const releasesRes = await axios.get(
        `https://musicbrainz.org/ws/2/release`,
        {
          params: { "release-group": mbid, fmt: "json" },
          headers: { "User-Agent": "YourApp/1.0 (youremail@example.com)" },
        }
      );
      for (const rel of releasesRes.data.releases) {
        const detail = await axios.get(
          `https://musicbrainz.org/ws/2/release/${rel.id}`,
          {
            params: { inc: "recordings", fmt: "json" },
            headers: { "User-Agent": "YourApp/1.0" },
          }
        );
        const t = detail.data.media?.[0]?.tracks;
        if (t?.length) {
          tracks = t.map((tr: any) => ({
            title: tr.title,
            length: tr.length ?? tr.recording?.length ?? 0,
          }));
          break;
        }
      }
      albumRating = +(Math.random() * 2 + 3).toFixed(1);
      trackRatings = tracks.map(() => +(Math.random() * 2 + 3).toFixed(1));
    } catch (e) {
      error = "Error loading album or tracklist";
    }
    loading = false;
  });
</script>

<div
  class="min-h-screen bg-gradient-to-br from-zinc-900 via-zinc-950 to-zinc-900 text-zinc-100 px-0 py-0 font-mono flex items-center justify-center"
>
  {#if loading}
    <div class="flex justify-center items-center w-full h-screen">
      <div
        class="animate-spin inline-block w-12 h-12 border-4 border-zinc-500 border-t-zinc-200 rounded-full"
      ></div>
    </div>
  {:else if error}
    <div class="text-center text-red-400 text-xl py-20 w-full">{error}</div>
  {:else}
    <div
      class="w-full max-w-7xl h-[90vh] bg-zinc-800/90 rounded-3xl shadow-2xl p-0 md:p-0 flex flex-col md:flex-row overflow-hidden border border-zinc-700"
    >
      <div
        class="flex flex-col items-center md:items-start md:w-[400px] bg-zinc-900/80 p-10 gap-8 border-r border-zinc-700"
      >
        <img
          src={cover}
          alt="Album cover"
          class="w-52 h-52 md:w-72 md:h-72 object-cover rounded-2xl shadow-lg mb-4"
        />
        <div class="w-full">
          <h1 class="text-4xl font-bold mb-3 leading-tight">{albumTitle}</h1>
          <p class="text-zinc-400 text-2xl mb-6">{artist}</p>
          <div class="flex items-center gap-3 mb-4">
            <span class="text-yellow-400 text-3xl">★</span>
            <span class="font-semibold text-2xl"
              >{formatRating(albumRating)}</span
            >
            <span class="text-zinc-400 text-base ml-2">Album rating</span>
          </div>
          <button
            on:click={openReviewModal}
            class="w-full bg-yellow-500 hover:bg-yellow-600 hover:scale-[1.02] text-black font-bold py-3 px-6 rounded-lg shadow-lg transition-all duration-200 cursor-pointer active:scale-[0.98] active:bg-yellow-700"
          >
            Write review
          </button>
        </div>
      </div>
      <div class="flex-1 p-0 flex flex-col h-full max-h-full overflow-y-auto">
        <div class="sticky top-0 z-20 bg-zinc-800 border-b border-zinc-700">
          <h2 class="text-3xl font-semibold mb-0 text-zinc-200 px-10 py-4">
            Tracklist
          </h2>
        </div>
        <ol class="space-y-3 mt-4 px-10">
          {#each tracks as track, idx}
            <li
              class="flex justify-between items-center text-lg md:text-xl text-zinc-300 py-3 px-4 rounded-lg hover:bg-zinc-900/70 transition-colors"
            >
              <span class="flex items-center gap-4">
                <span class="text-zinc-500 w-10 text-right"
                  >{(idx + 1).toString().padStart(2, "0")}</span
                >
                <span class="truncate max-w-[320px] md:max-w-[500px]"
                  >{track.title}</span
                >
              </span>
              <span class="flex items-center gap-3 min-w-[110px] justify-end">
                <span class="text-zinc-500 font-mono"
                  >{formatTime(track.length)}</span
                >
                <span class="text-yellow-400">★</span>
                <span class="text-zinc-400 font-mono"
                  >{formatRating(trackRatings[idx])}</span
                >
              </span>
            </li>
          {/each}
        </ol>
      </div>
    </div>

    {#if isReviewModalOpen}
      <div
        class="fixed inset-0 bg-black/70 flex items-center justify-center z-50 p-4"
        role="dialog"
        tabindex="0"
        aria-labelledby="review-modal-title"
        on:click|self={() => (isReviewModalOpen = false)}
        on:keydown={(e) => e.key === 'Escape' && (isReviewModalOpen = false)}
      >
        <div
          class="bg-zinc-800 rounded-xl p-8 w-full max-w-4xl max-h-[90vh] overflow-y-auto border border-zinc-700"
        >
          <h2 id="review-modal-title" class="text-2xl font-bold mb-6">Write Album Review</h2>

          {#if reviewError}
            <div role="alert" class="bg-red-900/50 border border-red-700 text-red-200 p-4 rounded-lg mb-6">
              {reviewError}
            </div>
          {/if}

          <div class="space-y-6">
            <div>
              <label for="album-rating" class="block text-zinc-400 mb-2">Album Rating</label>
              <div id="album-rating">
                <StarRating
                  rating={userAlbumRating}
                  onChange={(value) => (userAlbumRating = value)}
                />
              </div>
            </div>

            <div>
              <span class="block text-zinc-400 mb-4">Track Ratings</span>
              <div class="space-y-4">
                {#each tracks as track, idx}
                  <div class="flex justify-between items-center">
                    <span class="text-zinc-300">{track.title}</span>
                    <StarRating
                      rating={userTrackRatings[idx]}
                      onChange={(value) => userTrackRatings[idx] = value}
                    />
                  </div>
                {/each}
              </div>
            </div>

            <div>
              <label for="review-text" class="block text-zinc-400 mb-2"
                >Review Text (minimum 300 characters)</label
              >
              <textarea
                id="review-text"
                bind:value={reviewText}
                class="w-full h-48 bg-zinc-900 text-zinc-100 rounded-lg p-4 border border-zinc-700 focus:border-yellow-500 focus:ring-1 focus:ring-yellow-500 outline-none"
                placeholder="Write your thoughts about the album..."
              ></textarea>
              <p class="text-sm text-zinc-500 mt-2">
                {reviewText.length} / 300 characters
              </p>
            </div>

            <div class="flex justify-end gap-4">
              <button
                on:click={() => (isReviewModalOpen = false)}
                class="px-6 py-2 rounded-lg border border-zinc-600 text-zinc-300 hover:bg-zinc-700 transition-colors"
              >
                Cancel
              </button>
              <button
                on:click={submitReview}
                disabled={isSubmitting}
                class="px-6 py-2 rounded-lg bg-yellow-500 text-black font-semibold hover:bg-yellow-600 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {isSubmitting ? 'Submitting...' : 'Submit Review'}
              </button>
            </div>
          </div>
        </div>
      </div>
    {/if}
  {/if}
</div>

{#if toast}
  <Toast
    type={toast.type}
    message={toast.message}
    onClose={() => toast = null}
  />
{/if}
